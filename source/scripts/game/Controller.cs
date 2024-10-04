using Godot;

namespace Party.Game.Experience;

public sealed partial class Controller : Node
{
    public Vector2 Position
    {
        get => position;
        set
        {
            if (position.Equals(value))
            {
                return;
            }

            CallDeferred(MethodName.EmitSignal, SignalName.PositionChanged, position = value);
        }
    }

    public Action Current => current;

    private Action current;
    private Vector2 position;

    public override void _Process(double delta)
    {
        Position = GetViewport().GetMousePosition();
    }

    public void Perform(Action action)
    {
        if (current.Equals(action))
        {
            return;
        }

        current = action;

        CallDeferred(MethodName.EmitSignal, SignalName.ActionPerformed, (int)current);
    }

    public enum Action
    {
        Stand,
        Walk,
        Run,
        Jump,
        Sit,
        Squat,
        Kick,
        Punch,
        Wave,
        Maximum,
    }

    [Signal]
    public delegate void ActionPerformedEventHandler(Action action);

    [Signal]
    public delegate void PositionChangedEventHandler(Vector2 position);
}
