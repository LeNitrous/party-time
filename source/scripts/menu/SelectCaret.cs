using Godot;

namespace Party.Game.Menu;

[Tool]
public sealed partial class SelectCaret : Choice
{
    [Export(PropertyHint.ColorNoAlpha)]
    public Color Accent = Colors.White;

    private bool primed;
    private Label label;
    private Control caretL;
    private Control caretR;

    public override void _Ready()
    {
        label = GetNode<Label>("Label");
        caretL = GetNode<Control>("Left");
        caretR = GetNode<Control>("Right");
        base._Ready();
    }

    protected override void OnStateChanged(State state)
    {
        bool isActive = state.HasFlag(State.Selected) || state.HasFlag(State.Hovered);
        caretL.SelfModulate = caretR.SelfModulate = isActive ? Accent : inactive;
    }

    protected override void OnSelectChanged(int value)
    {
        if (Options.Count > value && value >= 0)
        {
            label.Text = Options[value];
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
                    if (caretL.GetGlobalRect().HasPoint(b.GlobalPosition))
                    {
                        Prev();
                    }
                    else if (caretR.GetGlobalRect().HasPoint(b.GlobalPosition))
                    {
                        Next();
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

    private static readonly Color inactive = Color.Color8(150, 150, 150);
}