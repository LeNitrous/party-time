using Party.Game.Experience.Directors;

namespace Party.Game.Menu;

public sealed partial class ButtonStartGameStandard : ButtonStartGame
{
    protected override GameDirector CreateDirector()
    {
        return new GameDirectorStandard();
    }
}
