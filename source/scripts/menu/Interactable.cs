using System;
using Godot;
using Party.Game.Input;

namespace Party.Game.Menu;

public abstract partial class Interactable : Control
{
    private State state;

    protected virtual bool CanBePressed => true;
    protected virtual bool CanBeHovered => true;
    protected virtual bool CanBeSelected => true;

    public override void _Notification(int what)
    {
        if (what == NotificationReady)
        {
            onStateChanged(state, true);
        }
    }

    protected virtual void OnStateChanged(State state)
    {
    }

    private void onGUIInput(InputEvent e)
    {
        if (e is InputEventMouseMotion m && state.HasFlag(State.Pressed))
        {
            if (!GetGlobalRect().HasPoint(m.GlobalPosition))
            {
                onRelease();
            }
        }

        if (e is InputEventMouseButton b && b.ButtonIndex == MouseButton.Left)
        {
            if (b.Pressed)
            {
                onPressed();
            }
            else
            {
                onRelease();
            }
        }

        if (e.IsActionPressed("ui_select"))
        {
            onPressed();
        }

        if (e.IsActionReleased("ui_select"))
        {
            onRelease();
        }
    }

    private void onPressed()
    {
        if (CanBePressed && FocusMode != FocusModeEnum.None)
        {
            onStateChanged(state | State.Pressed);
        }
    }

    private void onRelease()
    {
        if (state.HasFlag(State.Pressed))
        {
            onStateChanged(state & ~State.Pressed);
        }
    }

    private void onSelected()
    {
        if (CanBeSelected && FocusMode != FocusModeEnum.None)
        {
            onStateChanged(state | State.Selected);
        }
    }

    private void onDeselect()
    {
        if (state.HasFlag(State.Selected))
        {
            onStateChanged(state & ~State.Selected);
        }
    }

    private void onHoverGain()
    {
        if (CanBeHovered && FocusMode != FocusModeEnum.None)
        {
            onStateChanged(state | State.Hovered);
            InputManager.Current?.SetHovered(this);
        }
    }

    private void onHoverLost()
    {
        if (state.HasFlag(State.Hovered))
        {
            onStateChanged(state & ~State.Hovered);
            InputManager.Current?.SetHovered(null);
        }
    }

    private void onStateChanged(State next, bool force = false)
    {
        if (!force)
        {
            if (state == next)
            {
                return;
            }
        }

        OnStateChanged(state = next);
    }

    [Flags]
    protected enum State : byte
    {
        None = 1 << 0,
        Hovered = 1 << 1,
        Selected = 1 << 2,
        Pressed = 1 << 3,
    }
}
