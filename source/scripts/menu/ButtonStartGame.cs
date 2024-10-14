using Party.Game.Experience;
using Party.Game.Experience.Directors;

namespace Party.Game.Menu;

public abstract partial class ButtonStartGame : ButtonConfirmHandler
{
    protected sealed override void OnConfirm()
    {
        GameContext.Director = CreateDirector();
        SceneStack.Current?.Push("res://scenes/game.tscn");
    }

    protected abstract GameDirector CreateDirector();
}
