using Party.Game.Experience.Directors;

namespace Party.Game.Menu;

public sealed partial class ButtonStartGameEndurance : ButtonStartGame
{
    protected override GameDirector CreateDirector()
    {
        return new GameDirectorEndurance();
    }
}