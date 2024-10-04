using System;
using Godot;

namespace Party.Game.Experience.Events;

public sealed partial class GameEventInstruct : GameEvent
{
    public override double Duration => 10.0;

    private Controller.Action action;

    public override void _Ready()
    {
        action = (Controller.Action)Random.Shared.Next(1, (int)Controller.Action.Maximum);
        GetNode<Label>("%Action").Text = action.ToString();
    }

    public override void _Process(double delta)
    {
        if (Controller is null)
        {
            return;
        }

        if (Controller.Current == action)
        {
            Trigger(Completion.Win);
        }
    }
}
