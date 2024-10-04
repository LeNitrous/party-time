using Godot;

namespace Party.Game.Menu;

[Tool]
public partial class SelectOption : Control
{
    [Export]
    public string Text
    {
        get => this.GetValueAt<string>("%Label", Label.PropertyName.Text);
        set => this.SetValueAt("%Label", Label.PropertyName.Text, value);
    }

    [Export]
    public bool Selected
    {
        get => selected;
        set => setSelected(in value);
    }

    [Export]
    public bool Highligted
    {
        get => this.GetValueAt<bool>("%Shadow", PropertyName.Visible);
        set => this.SetValueAt("%Shadow", PropertyName.Visible, value);
    }

    private bool selected;
    private Control label;
    private Control border;
    private Control background;

    public override void _Ready()
    {
        update();
    }

    private void setSelected(in bool value)
    {
        if (selected == value)
        {
            return;
        }

        selected = value;

        if (IsNodeReady())
        {
            update();
        }
    }

    private void update()
    {
        label ??= GetNode<Control>("%Label");
        label.SelfModulate = selected ? new Color(0.08f, 0.08f, 0.08f) : new Color(0.49f, 0.584f, 0.671f);

        border ??= GetNode<Control>("%Border");
        border.Visible = selected;

        background ??= GetNode<Control>("%Background");
        background.SelfModulate = selected ? new Color(0.949f, 0.808f, 0.231f) : new Color(0.737f, 0.796f, 0.851f);
    }
}
