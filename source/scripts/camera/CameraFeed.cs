using System;
using Godot;

namespace Party.Game.Camera;

public abstract class CameraFeed : ICameraFeed
{
    public event Action<Image> OnFrame;

    public event Action OnStart;

    public event Action OnClose;

    public abstract string Name { get; }

    public abstract int Width { get; }

    public abstract int Height { get; }

    public abstract bool Accelerated { get; }

    public abstract void Start();

    public abstract void Close();

    protected void EmitFrame(Image image) => OnFrame?.Invoke(image);

    protected void EmitStart() => OnStart?.Invoke();

    protected void EmitClose() => OnClose?.Invoke();
}