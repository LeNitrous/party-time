using Party.Game.Config;

namespace Party.Game.Menu.Settings;

public sealed partial class SelectGameTimeshift : SelectChangeHandler
{
    public override void _Ready()
    {
        Parent.Selected = ConfigManager.Current?.GetValue("gameplay", "timeshift", true) ?? true ? 1 : 0;
    }

    protected override void OnCurrentChanged(int value)
    {
        ConfigManager.Current?.SetValue("gameplay", "timeshift", value == 1);
    }
}
