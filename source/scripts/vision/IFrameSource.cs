using System;
using Godot;

namespace Party.Game.Vision;

public interface IFrameSource
{
    bool Accelerated { get; }

    FrameSourceKind Kind { get; }

    event Action<Image> OnFrame;
}