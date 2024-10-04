using System;
using Godot;

namespace Party.Game.Experience.Events;

public abstract partial class GameEvent : Node
{
    public virtual double Duration { get; } = 0.0;

    public virtual Completion Timeout => Completion.LoseTimeout;

    public event Action<Completion> CompletionChanged;

    protected Controller Controller { get; private set; }

    private bool isCompletionSet;
    private Completion completion;

    public override void _Notification(int what)
    {
        if (what == NotificationParented)
        {
            Controller = GetParent().GetNodeOrNull<Controller>("%Controller");
        }

        if (what == NotificationUnparented)
        {
            Controller = null;
        }
    }

    protected void Trigger(Completion state)
    {
        if (isCompletionSet)
        {
            return;
        }

        CompletionChanged?.Invoke(state);

        isCompletionSet = true;
    }
}
