using Godot;

namespace Party.Game.Menu;

public abstract partial class ModalSignalHandler : Component<Modal>
{
    protected abstract void OnAccept();

    protected abstract void OnReject();

    protected sealed override void OnAttach()
    {
        Parent.Connect(Modal.SignalName.Accept, Callable.From(OnAccept));
        Parent.Connect(Modal.SignalName.Reject, Callable.From(OnReject));
    }

    protected sealed override void OnDetach()
    {
        Parent.Disconnect(Modal.SignalName.Accept, Callable.From(OnAccept));
        Parent.Disconnect(Modal.SignalName.Reject, Callable.From(OnReject));
    }
}
