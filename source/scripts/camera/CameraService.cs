using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using Party.Game.Vision;

namespace Party.Game.Camera;

public sealed partial class CameraService : Node, ICameraFeed, IFrameSource
{
    public static CameraService Current { get; private set; }

    public int Feed
    {
        get => feed.Current;
        set => feed.Current = value;
    }
    
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
    }

    public void RemoveFeed(CameraFeed feed)
    {
        feeds.Remove(feed);
    }

    public bool PermissionGranted()
    {
        return impl.PermissionGranted();
    }

    public Task<bool> RequestPermission()
    {
        return impl.RequestPermission();
    }

    event Action<Image> IFrameSource.OnFrame
    {
        add => feed.OnFrame += value;
        remove => feed.OnFrame -= value;
    }

     event Action<Image> ICameraFeed.OnFrame
    {
        add => feed.OnFrame += value;
        remove => feed.OnFrame -= value;
    }

    event Action ICameraFeed.OnStart
    {
        add => feed.OnStart += value;
        remove => feed.OnStart -= value;
    }

    event Action ICameraFeed.OnClose
    {
        add => feed.OnClose += value;
        remove => feed.OnClose -= value;
    }

    int ICameraFeed.Width => feed.Width;

    int ICameraFeed.Height => feed.Height;

    FrameSourceKind IFrameSource.Kind => FrameSourceKind.Stream;

    bool IFrameSource.Accelerated => feed.IsAccelerated();

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

        protected override bool Accelerated => current < owner.feeds.Count && ((Vision.IFrameSource)owner.feeds[current]).Accelerated;

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
        private readonly CameraService owner;

        public CameraFeedSwitchable(CameraService owner)
        {
            this.owner = owner;
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

            if (current >= owner.feeds.Count)
            {
                return;
            }

            owner.feeds[current].OnStart += EmitStart;
            owner.feeds[current].OnClose += EmitClose;
            owner.feeds[current].OnFrame += EmitFrame;
            owner.feeds[current].Start();

            hasStarted = true;
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
    }
}
