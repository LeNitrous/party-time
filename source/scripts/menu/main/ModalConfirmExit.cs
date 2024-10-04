namespace Party.Game.Menu.Main;

public sealed partial class ModalConfirmExit : ModalSignalHandler
{
    protected override void OnAccept()
    {
        GetTree().Quit();
    }

    protected override void OnReject()
    {
        Parent.Hide();
    }
}
