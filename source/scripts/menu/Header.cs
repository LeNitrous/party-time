using Godot;

namespace Party.Game.Menu;

[Tool]
public partial class Header : Control
{
    [ExportGroup("Text")]
    [Export]
    public string Title
    {
        get => this.GetValueAt<string>("%Title", Label.PropertyName.Text);
        set => this.SetValueAt("%Title", Label.PropertyName.Text, value);
    }

    [Export]
    public string Subtitle
    {
        get => this.GetValueAt<string>("%Subtitle", Label.PropertyName.Text);
        set
        {
            this.SetValueAt("%Subtitle", Label.PropertyName.Text, value);
            this.SetValueAt("%Separator", PropertyName.Visible, !string.IsNullOrWhiteSpace(value));
        }
    }

    [ExportGroup("Color")]
    [Export]
    public Color Color
    {
        get => this.GetValueAt<Color>("%Colorable", PropertyName.SelfModulate);
        set
        {
            this.SetValueAt("%Colorable", PropertyName.SelfModulate, value);
            this.SetValueAt("%Title", PropertyName.SelfModulate, value);
        }
    }
}
