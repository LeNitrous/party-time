using Party.Game.Config;
using Party.Game.Experience.Directors;

namespace Party.Game.Menu.Settings;

public sealed partial class SelectGameLength : SelectChangeHandler
{
    public override void _Ready()
    {
        Parent.Selected = (GameDirectorStandard.GameLength)(ConfigManager.Current?.GetValue("gameplay", "length", 1) ?? 1) switch
        {
            GameDirectorStandard.GameLength.Short => 0,
            GameDirectorStandard.GameLength.Medium => 1,
            _ => 2,
        };
    }

    protected override void OnCurrentChanged(int value)
    {
        var mode = value switch
        {
            0 => GameDirectorStandard.GameLength.Short,
            1 => GameDirectorStandard.GameLength.Medium,
            _ => GameDirectorStandard.GameLength.Long,
        };

        ConfigManager.Current?.SetValue("gameplay", "length", mode);
    }
}
