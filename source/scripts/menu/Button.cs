using System;
using Godot;
using NathanHoad;

namespace Party.Game.Menu;

[Tool]
public partial class Button : Interactable
{
    [ExportGroup("Presentation")]
    [Export]
    public string Text
    {
        get => this.GetValueAt<string>("%Label", Label.PropertyName.Text);
        set => this.SetValueAt("%Label", Label.PropertyName.Text, value);
    }

    [Export]
    public Texture2D Icon
    {
        get => this.GetValueAt<Texture2D>("%Icon", TextureRect.PropertyName.Texture);
        set
        {
            this.SetValueAt("%Icon", TextureRect.PropertyName.Texture, value);
            this.SetValueAt("%Icon", Control.PropertyName.Visible, value is not null);
        }
    }

    [ExportGroup("Colors")]
    [Export(PropertyHint.ColorNoAlpha)]
    public Color Default
    {
        get => color;
        set
        {
            value.ToHsv(out float h, out float s, out float v);

            if (s > 0.3f)
            {
                Hovered = Color.FromHsv(h, s - 0.1f, v);
                Pressed = Color.FromHsv(h, s - 0.2f, v);
            }
            else
            {
                Hovered = Color.FromHsv(h, s, v + 0.1f);
                Pressed = Color.FromHsv(h, s, v + 0.2f);
            }
            
            this.SetValueAt("%Background", ColorRect.PropertyName.Color, color = value);
        }
    }

    public Color Hovered { get; private set; }

    public Color Pressed { get; private set; }

    private bool primed;
    private string text;
    private Color color;
    private ColorRect accent;
    private ColorRect padder;
    private Texture2D texture;
    private AudioStream effect;
    private PanelContainer border;

    protected override void OnStateChanged(State state)
    {
         accent = GetNode<ColorRect>("%Background");

        if (state.HasFlag(State.Pressed))
        {
            accent.Color = Pressed;
        }
        else if (state.HasFlag(State.Hovered))
        {
            accent.Color = Hovered;
        }
        else
        {
            accent.Color = Default;
        }

        border ??= GetNode<PanelContainer>("%Border");
        padder ??= GetNodeOrNull<ColorRect>("Content/Padder/Fill");
        effect ??= GD.Load<AudioStream>("res://sounds/effects/ui_click.ogg");

        var color = state.HasFlag(State.Selected) ? Hovered : new Color(0.086f, 0.086f, 0.086f);

        if (border is not null)
        {
            border.SelfModulate = color;
        }

        if (padder is not null)
        {
            padder.Color = color;
        }
    }

    private void onButtonGUIInput(InputEvent e)
    {
        if (e is InputEventMouseButton b)
        {
            if (b.Pressed)
            {
                primed = true;
                doSoundEffect();
            }

            if (primed && !b.Pressed)
            {
                primed = false;
                EmitSignal(SignalName.Confirm);
                CallDeferred(MethodName.release);
            }
        }

        if (e is InputEventMouseMotion m)
        {
            if (primed && !GetGlobalRect().HasPoint(m.GlobalPosition))
            {
                primed = false;
                CallDeferred(MethodName.release);
            }
        }

        if (e.IsActionPressed("ui_select"))
        {
            primed = true;
            doSoundEffect();
        }

        if (primed && e.IsActionReleased("ui_select"))
        {
            primed = false;
            EmitSignal(SignalName.Confirm);
            CallDeferred(MethodName.release);
        }
    }

    private void release()
    {
        if (IsInsideTree())
        {
            CallDeferred(MethodName.ReleaseFocus);
        }
    }

    private void doSoundEffect()
    {
        SoundManager.PlayUISoundWithPitch(effect, Mathf.Remap(Random.Shared.NextSingle(), 0.0f, 1.0f, 0.8f, 1.2f), "Effects");
    }

    [Signal]
    public delegate void ConfirmEventHandler();
}
