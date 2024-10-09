using System;
using Godot;
using Party.Game.Vision;

namespace Party.Game.Camera;

public abstract class CameraFeed : ICameraFeed, IFrameSource
{
    public event Action<Image> OnFrame;

    public event Action OnStart;

    public event Action OnClose;

    public abstract string Name { get; }

    public abstract int Width { get; }

    public abstract int Height { get; }

    protected abstract bool Accelerated { get; }

    public abstract void Start();

    public abstract void Close();

    protected void EmitFrame(Image image) => OnFrame?.Invoke(image);

    protected void EmitStart() => OnStart?.Invoke();

    protected void EmitClose() => OnClose?.Invoke();

    bool Vision.IFrameSource.Accelerated => Accelerated;

    FrameSourceKind Vision.IFrameSource.Kind => FrameSourceKind.Stream;
}