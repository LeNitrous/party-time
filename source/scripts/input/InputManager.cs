using System;
using Godot;

namespace Party.Game.Input;

public partial class InputManager : Node
{
    /// <summary>
    /// The singleton instance.
    /// </summary>
    public static InputManager Current { get; private set; }

    /// <summary>
    /// The current input schema active.
    /// </summary>
    public InputSchema Schema { get; private set; }

    /// <summary>
    /// The current focused control.
    /// </summary>
    public NodePath Focused { get; private set; }

    /// <summary>
    /// The current hovered control.
    /// </summary>
    public NodePath Hovered { get; private set; }

    /// <summary>
    /// The first selected control.
    /// </summary>
    [Export]
    public Control FirstSelected { get; set; }

    private Viewport viewport;

    public override void _Ready()
    {
        if (Current is null)
        {
            Current = this;
            Current.EmitSignal(SignalName.SchemaChanged, (int)(Current.Schema = OS.HasFeature("mobile") ? InputSchema.Touch : InputSchema.Mouse));
        }
        else
        {
            throw new InvalidOperationException();
        }

        viewport = GetViewport();
    }

    public override void _ExitTree()
    {
        if (Current is not null && Current == this)
        {
            Current = null;
        }
    }

    public override void _Process(double delta)
    {
        var current = viewport.GuiGetFocusOwner();

        if (current is null && Focused is not null)
        {
            EmitSignal(SignalName.FocusChanged, Focused = null);
        }
        else if (current is not null)
        {
            var path = current.GetPath();

            if (path != Focused)
            {
                EmitSignal(SignalName.FocusChanged, Focused = path);
            }
        }
    }

    public override void _Input(InputEvent input)
    {
        var next = InputSchema.None;

        if (input.IsActionPressed("ui_focus_next") || input.IsActionPressed("ui_focus_prev") || input.IsActionPressed("ui_up") || input.IsActionPressed("ui_down") || input.IsActionPressed("ui_left") || input.IsActionPressed("ui_right"))
        {
            if (FirstSelected is not null && FirstSelected.IsInsideTree() && (Focused is null || Focused.IsEmpty))
            {
                FirstSelected.CallDeferred(Control.MethodName.GrabFocus);
            }
        }

        if (input is InputEventMouseMotion m)
        {
            if (Focused is not null && !Focused.IsEmpty)
            {
                var node = GetNode<Control>(Focused);
                var rect = node.GetGlobalRect();

                if (node.IsInsideTree() && !rect.HasPoint(m.GlobalPosition))
                {
                    node.CallDeferred(Control.MethodName.ReleaseFocus);
                }
            }
        }

        if (input is InputEventJoypadButton or InputEventJoypadMotion)
        {
            string name = Godot.Input.GetJoyName(input.Device);

            if (input is InputEventJoypadMotion motion && motion.AxisValue < 0.25f)
            {
                return;
            }

            if (name.StartsWith("PS"))
            {
                next = InputSchema.PlayStation;
            }
            else if (name.StartsWith("Nintendo"))
            {
                next = InputSchema.Nintendo;
            }
            else if (name.StartsWith("Xbox"))
            {
                next = InputSchema.Xbox;
            }
            else
            {
                next = InputSchema.Gamepad;
            }
        }

        if (input is InputEventMouse or InputEventKey)
        {
            next = InputSchema.Mouse;
        }

        if (input is InputEventKey)
        {
            next = InputSchema.Keyboard;
        }

        if (input is InputEventScreenTouch)
        {
            next = InputSchema.Touch;
        }

        if (Schema != next)
        {
            EmitSignal(SignalName.SchemaChanged, (int)(Schema = next));
        }
    }

    public void SetHovered(Control control)
    {
        var path = control?.GetPath();

        if (path == Hovered)
        {
            return;
        }

        EmitSignal(SignalName.HoverChanged, Hovered = path);
    }

    [Signal]
    public delegate void FocusChangedEventHandler(NodePath focused);

    [Signal]
    public delegate void HoverChangedEventHandler(NodePath hovered);

    [Signal]
    public delegate void SchemaChangedEventHandler(InputSchema schema);
}
