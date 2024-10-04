using Godot;

namespace Party.Game.Experience;

[Tool]
public sealed partial class HUDLabel : Control
{
    [Export]
    public string Label
    {
        get => this.GetValueAt<string>("%Label", Godot.Label.PropertyName.Text);
        set => this.SetValueAt("%Label", Godot.Label.PropertyName.Text, value);
    }

    [Export]
    public string Value
    {
        get => this.GetValueAt<string>("%Value", Godot.Label.PropertyName.Text);
        set => this.SetValueAt("%Value", Godot.Label.PropertyName.Text, value);
    }
}
