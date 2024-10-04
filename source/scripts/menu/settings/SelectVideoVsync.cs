using Party.Game.Config;

namespace Party.Game.Menu.Settings;

public sealed partial class SelectVideoVsync : SelectChangeHandler
{
    public override void _Ready()
    {
        Parent.Selected = ConfigManager.Current?.GetValue("display", "window/vsync/vsync_mode", 1) ?? 1;
    }

    protected override void OnCurrentChanged(int value)
    {
        ConfigManager.Current?.SetValue("display", "window/vsync/vsync_mode", value);
    }
}
