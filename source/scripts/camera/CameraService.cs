using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

namespace Party.Game.Camera;

public sealed partial class CameraService : Node, ICameraFeed
{
    public static CameraService Current { get; private set; }

    public int Feed
    {
        get => feed.Current;
        set => feed.Current = value;
    }

    public event Action<CameraFeed> OnFeedAdded;

    public event Action<CameraFeed> OnFeedRemoved;

    public event Action<Image> OnFrame
    {
        add => feed.OnFrame += value;
        remove => feed.OnFrame -= value;
    }

    public event Action OnStart
    {
        add => feed.OnStart += value;
        remove => feed.OnStart -= value;
    }

    public event Action OnClose
    {
        add => feed.OnClose += value;
        remove => feed.OnClose -= value;
    }

    public bool Accelerated => feed.Accelerated;
    
    private Impl impl;
    private CameraFeedSwitchable feed;
    private readonly List<CameraFeed> feeds = new List<CameraFeed>();

    public override void _Ready()
    {
        if (Current is null)
        {
            Current = this;
        }

        feed = new CameraFeedSwitchable(this);
        impl = OS.HasFeature("mobile") ? new CameraServiceMobile(this) : new CameraServiceDesktop(this);
    }

    public override void _ExitTree()
    {
        if (Current == this)
        {
            Current = null;
        }

        impl.Dispose();
    }

    public void Start()
    {
        feed.Start();
    }

    public void Close()
    {
        feed.Close();
    }

    public void AddFeed(CameraFeed feed)
    {
        feeds.Add(feed);
        OnFeedAdded?.Invoke(feed);
    }

    public void RemoveFeed(CameraFeed feed)
    {
        feeds.Remove(feed);
        OnFeedRemoved?.Invoke(feed);
    }

    public bool PermissionGranted()
    {
        return impl.PermissionGranted();
    }

    public Task<bool> RequestPermission()
    {
        return impl.RequestPermission();
    }

    int ICameraFeed.Width => feed.Width;

    int ICameraFeed.Height => feed.Height;

    public abstract class Impl : IDisposable
    {
        public abstract bool PermissionGranted();

        public abstract Task<bool> RequestPermission();

        public abstract void Dispose();
    }

    private sealed class CameraFeedSwitchable : CameraFeed
    {
        public override string Name => current < owner.feeds.Count ? owner.feeds[current].Name : string.Empty;

        public override int Width => current < owner.feeds.Count ? owner.feeds[current].Width : 0;

        public override int Height => current < owner.feeds.Count ? owner.feeds[current].Height : 0;

        public override bool Accelerated => current < owner.feeds.Count && owner.feeds[current].Accelerated;

        public int Current
        {
            get => current;
            set
            {
                if (current == value)
                {
                    return;
                }

                Close();
                current = Math.Clamp(value, 0, owner.feeds.Count - 1);
                Start();
            }
        }

        private int current;
        private bool hasStarted;
        private bool hasStartAttempted;
        private readonly CameraService owner;

        public CameraFeedSwitchable(CameraService owner)
        {
            this.owner = owner;
            this.owner.OnFeedAdded += onFeedAdded;
        }

        public bool IsAccelerated()
        {
            return Accelerated;
        }

        public override void Start()
        {
            if (hasStarted)
            {
                return;
            }

            hasStartAttempted = true;

            if (current >= owner.feeds.Count)
            {
                return;
            }

            owner.feeds[current].OnStart += EmitStart;
            owner.feeds[current].OnClose += EmitClose;
            owner.feeds[current].OnFrame += EmitFrame;
            owner.feeds[current].Start();

            hasStarted = true;
            hasStartAttempted = false;
        }

        public override void Close()
        {
            if (!hasStarted)
            {
                return;
            }

            if (current >= owner.feeds.Count)
            {
                return;
            }

            owner.feeds[current].Close();
            owner.feeds[current].OnStart -= EmitStart;
            owner.feeds[current].OnClose -= EmitClose;
            owner.feeds[current].OnFrame -= EmitFrame;

            hasStarted = false;
        }

        private void onFeedAdded(CameraFeed feed)
        {
            if (hasStartAttempted)
            {
                Start();
            }
        }
    }
}
