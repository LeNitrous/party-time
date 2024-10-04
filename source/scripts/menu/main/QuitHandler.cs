using Godot;

namespace Party.Game.Menu.Main;

public sealed partial class QuitHandler : Node
{
    public override void _Notification(int what)
    {
        switch (what)
        {
            case (int)NotificationWMCloseRequest:
            case (int)NotificationWMGoBackRequest:
                handleQuitRequest();
                break;
        }
    }

    private void handleQuitRequest()
    {
        var modal = GetNode<Control>("%Modal");

        if (modal.Visible)
        {
            modal.GetParent().EmitSignal(Modal.SignalName.Accept);
        }
        else
        {
            modal.Show();
        }
    }
}
