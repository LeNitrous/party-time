using Party.Game.Config;
using Party.Game.Experience.Managers;

namespace Party.Game.Menu.Settings;

public sealed partial class SelectGameAnnouncer : SelectChangeHandler
{
    public override void _Ready()
    {
        Parent.Selected = (GameCountdown.Announcer)(ConfigManager.Current?.GetValue("gameplay", "announcer", 2) ?? 2) switch
        {
            GameCountdown.Announcer.Male => 0,
            GameCountdown.Announcer.Female => 1,
            _ => 2,
        };
    }

    protected override void OnCurrentChanged(int value)
    {
        var mode = value switch
        {
            0 => GameCountdown.Announcer.Male,
            1 => GameCountdown.Announcer.Female,
            _ => GameCountdown.Announcer.Random,
        };

        ConfigManager.Current?.SetValue("gameplay", "announcer", mode);
    }
}