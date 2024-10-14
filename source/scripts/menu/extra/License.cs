using Godot;

namespace Party.Game.Menu.Extra;

public sealed partial class License : Node
{
    public override void _Ready()
    {
        using var file = FileAccess.Open("res://THIRDPARTY-LICENSE.txt", FileAccess.ModeFlags.Read);
        GetNode<Label>("%Label").Text = file.GetAsText();
    }
}