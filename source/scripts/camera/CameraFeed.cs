using System;
using GDExtension.Wrappers;
using Godot;

namespace Party.Game.Camera;

public abstract class CameraFeed : ICameraFeed
{
    public event Action<MediaPipeImage> OnFrame;

    public event Action OnStart;

    public event Action OnClose;

    public abstract string Name { get; }

    public abstract int Width { get; }

    public abstract int Height { get; }

    public abstract void Start();

    public abstract void Close();

    protected void EmitFrame(MediaPipeImage image) => OnFrame?.Invoke(image);

    protected void EmitStart() => OnStart?.Invoke();

    protected void EmitClose() => OnClose?.Invoke();
}