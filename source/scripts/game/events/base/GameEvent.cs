using System;
using GDExtension.Wrappers;
using Godot;

namespace Party.Game.Experience.Events;

public abstract partial class GameEvent : Node
{
    public virtual double Duration { get; } = 0.0;

    public event Action<Completion> CompletionChanged;

    protected bool IsCompletionSet { get; private set; }

    public virtual Completion GetCompletionOnTimeout()
    {
        return Completion.LoseTimeout;
    }

    public virtual void CameraFrameReceived(MediaPipeImage image)
    {
    }

    protected void Trigger(Completion state)
    {
        if (IsCompletionSet)
        {
            return;
        }

        CompletionChanged?.Invoke(state);

        IsCompletionSet = true;
    }
}
