using Party.Game.Experience.Directors;

namespace Party.Game.Menu;

public sealed partial class ButtonStartGameTutorial : ButtonStartGame
{
    protected override GameDirector CreateDirector()
    {
        return new GameDirectorTutorial();
    }
}
