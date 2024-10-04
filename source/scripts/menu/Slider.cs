using System;
using Godot;

namespace Party.Game.Menu;

[Tool]
public partial class Slider : Interactable
{
    [Export]
    public string Text
    {
        get => this.GetValueAt<string>("%Label", Label.PropertyName.Text);
        set => this.SetValueAt("%Label", Label.PropertyName.Text, value);
    }

    [Export]
    public float Maximum
    {
        get => maximum;
        set => onMaximumChanged(in value);
    }

    [Export]
    public float Minimum
    {
        get => minimum;
        set => onMinimumChanged(in value);
    }

    [Export]
    public float Current
    {
        get => current;
        set => onCurrentChanged(in value);
    }

    [Export]
    public float Step { get; set; } = 0.01f;

    private bool pressed;
    private string text;
    private float current;
    private float minimum = 0.0f;
    private float maximum = 1.0f;
    private Control bar;
    private Label value;
    private Control knob;
    private Control area;
    private Control fill;
    private Control border;
    private Control knobBack;

    public override void _Ready()
    {
        setPresentationToCurrent();
    }

    public override bool _Set(StringName property, Variant value)
    {
        if (property == PropertyName.Step)
        {
            Step = Mathf.Clamp(value.AsSingle(), 0.1f, Maximum);
            return true;
        }

        if (property == PropertyName.Minimum)
        {
            Minimum = Mathf.Clamp(value.AsSingle(), 0, Maximum);
            return true;
        }

        if (property == PropertyName.Current)
        {
            Current = Mathf.Clamp(value.AsSingle(), Minimum, Maximum);
            return true;
        }

        return false;
    }

    protected override void OnStateChanged(State state)
    {
        knobBack ??= GetNode<Control>("Body/Drag/Knob/Back");
        knobBack.Visible = state.HasFlag(State.Selected) || state.HasFlag(State.Hovered);
    }

    private void onSliderGUIInput(InputEvent e)
    {
        if (e is InputEventMouseButton b)
        {
            if (b.ButtonIndex == MouseButton.Left)
            {
                if (b.Pressed)
                {
                    if (knob.GetGlobalRect().HasPoint(b.GlobalPosition) || (bar ??= GetNode<Control>("Body/Base")).GetGlobalRect().HasPoint(b.GlobalPosition))
                    {
                        area ??= GetNode<Control>("Body/Drag");

                        var rect = area.GetRect();
                        var mpos = area.GetLocalMousePosition();

                        pressed = true;
                        Current = Maximum * (mpos.X / rect.Size.X);
                    }
                }
                else
                {
                    pressed = false;
                    CallDeferred(MethodName.ReleaseFocus);
                }
            }

            if (b.ButtonIndex == MouseButton.WheelUp)
            {
                Current += Step;
            }

            if (b.ButtonIndex == MouseButton.WheelDown)
            {
                Current -= Step;
            }
        }

        if (e is InputEventMouseMotion && pressed)
        {
            var rect = area.GetRect();
            var mpos = area.GetLocalMousePosition();
            Current = Maximum * (mpos.X / rect.Size.X);
        }

        if (e.IsAction("ui_left"))
        {
            Current -= Step;
        }

        if (e.IsAction("ui_right"))
        {
            Current += Step;
        }
    }

    private void onCurrentChanged(in float value)
    {
        if (current == value)
        {
            return;
        }

        EmitSignal(SignalName.CurrentChanged, current = Math.Clamp(value, Minimum, Maximum));

        setPresentationToCurrent();
    }

    private void onMaximumChanged(in float value)
    {
        if (maximum == value)
        {
            return;
        }

        maximum = value;

        if (value <= maximum)
        {
            return;
        }

        EmitSignal(SignalName.CurrentChanged, current = Math.Clamp(value, Minimum, Maximum));

        setPresentationToCurrent();
    }

    private void onMinimumChanged(in float value)
    {
        if (minimum == value)
        {
            return;
        }

        minimum = value;

        if (value >= current)
        {
            return;
        }

        EmitSignal(SignalName.CurrentChanged, current = Math.Clamp(value, Minimum, Maximum));

        setPresentationToCurrent();
    }

    private void setPresentationToCurrent()
    {
        if (!IsNodeReady())
        {
            return;
        }

        float percentage = Current / Maximum;

        knob ??= GetNode<Control>("Body/Drag/Knob");
        knob.AnchorLeft = percentage;
        knob.AnchorRight = percentage;

        fill ??= GetNode<Control>("Body/Base/Fill/Gradient");
        fill.AnchorRight = percentage;

        value ??= GetNode<Label>("%Value");
        value.Text = Current.ToString(@"##0");
    }

    [Signal]
    public delegate void CurrentChangedEventHandler(float value);
}
