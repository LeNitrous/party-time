using System;
using Godot;

namespace Party.Game.Camera;

public interface ICameraFeed
{
    int Width { get; }

    int Height { get; }

    event Action OnStart;

    event Action OnClose;

    event Action<Image> OnFrame;
}