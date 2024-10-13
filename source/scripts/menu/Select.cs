using Godot;
using Godot.Collections;

namespace Party.Game.Menu;

[Tool]
public sealed partial class Select : Choice
{
    [Export]
    public string Text
    {
        get => this.GetValueAt<string>("%Label", Label.PropertyName.Text);
        set => this.SetValueAt("%Label", Label.PropertyName.Text, value);
    }

    private bool primed;
    private Control flow;
    private PackedScene pack;

    public override void _Ready()
    {
        flow = GetNode<Control>("%Flow");
        pack = GD.Load<PackedScene>("res://scenes/components/ui_button_option.tscn");
        base._Ready();
    }

    protected override void OnStateChanged(State state)
    {
        bool active = state.HasFlag(State.Selected) || state.HasFlag(State.Hovered);

        for (int i = 0; i < flow.GetChildCount(); i++)
        {
            var item = flow.GetChild<SelectOption>(i);
            item.Highligted = item.Selected && active;
        }
    }

    protected override void OnSelectChanged(int value)
    {
        for (int i = 0; i < flow.GetChildCount(); i++)
        {
            var item = flow.GetChild<SelectOption>(i);
            item.Selected = value == i;
            item.Highligted = value == i;
        }
    }

    protected override void OnOptionChanged(Array<string> value)
    {
        foreach (var child in flow.GetChildren())
        {
            child.QueueFree();
        }

        for (int i = 0; i < value.Count; i++)
        {
            var button = pack.Instantiate<SelectOption>();
            button.Text = value[i];
            button.Selected = Selected == i;
            button.Highligted = false;
            button.SizeFlagsVertical = SizeFlags.ShrinkCenter;
            button.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            flow.AddChild(button);
        }
    }

    private void onSelectGUIInput(InputEvent e)
    {
        if (e is InputEventMouseButton b)
        {
            if (b.ButtonIndex == MouseButton.Left)
            {
                if (b.Pressed)
                {
                    primed = true;
                }

                if (primed && !b.Pressed)
                {
                    for (int i = 0; i < flow.GetChildCount(); i++)
                    {
                        if (flow.GetChild<SelectOption>(i).GetGlobalRect().HasPoint(b.GlobalPosition))
                        {
                            Selected = i;
                            break;
                        }
                    }

                    primed = false;
                    CallDeferred(MethodName.ReleaseFocus);
                }
            }
        }

        if (e is InputEventMouseMotion m)
        {
            if (primed && !GetGlobalRect().HasPoint(m.GlobalPosition))
            {
                primed = false;
                CallDeferred(MethodName.ReleaseFocus);
            }
        }

        if (e.IsActionReleased("ui_left"))
        {
            Prev();
        }

        if (e.IsActionReleased("ui_right"))
        {
            Next();
        }
    }
}
