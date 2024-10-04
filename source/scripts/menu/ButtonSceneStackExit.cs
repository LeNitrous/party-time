namespace Party.Game.Menu;

public sealed partial class ButtonSceneStackExit : ButtonConfirmHandler
{
    protected override void OnConfirm()
    {
        if (SceneStack.Current is null)
        {
            return;
        }

        SceneStack.Current.Exit();
    }
}
