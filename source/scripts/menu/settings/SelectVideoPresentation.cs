using Godot;
using Party.Game.Config;

namespace Party.Game.Menu.Settings;

public sealed partial class SelectVideoPresentation : SelectChangeHandler
{
    public override void _Ready()
    {
        Parent.Selected = (DisplayServer.WindowMode)(ConfigManager.Current?.GetValue("display", "window/size/mode", 0) ?? 0) switch
        {
            DisplayServer.WindowMode.Windowed => 0,
            _ => 1,
        };
    }

    protected override void OnCurrentChanged(int value)
    {
        var mode = value switch
        {
            0 => DisplayServer.WindowMode.Windowed,
            _ => DisplayServer.WindowMode.Fullscreen,
        };

        ConfigManager.Current?.SetValue("display", "window/size/mode", mode);
    }
}
