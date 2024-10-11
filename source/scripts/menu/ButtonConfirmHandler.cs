using Godot;

namespace Party.Game.Menu;

public abstract partial class ButtonConfirmHandler : Component<Button>
{
    protected abstract void OnConfirm();

    protected sealed override void OnAttach()
    {
        if (!Engine.IsEditorHint())
        {
            Parent.Connect(Button.SignalName.Confirm, Callable.From(OnConfirm));
        }
    }

    protected sealed override void OnDetach()
    {
        if (!Engine.IsEditorHint())
        {
            Parent.Disconnect(Button.SignalName.Confirm, Callable.From(OnConfirm));
        }
    }
}
