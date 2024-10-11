using Godot;
using Party.Game.Input;

namespace Party.Game.Menu;

[Tool]
public sealed partial class Prompt : Control, IModal
{
    private Button accept;
    private Button reject;
    private Control previousFirstSelected;

    public override void _Ready()
    {
        if (SceneStack.Current is not null)
        {
            SceneStack.Current.Modal = this;
        }

        if (InputManager.Current is not null)
        {
            previousFirstSelected = InputManager.Current.FirstSelected;
        }

        accept = GetNode<Button>("%Accept");
        reject = GetNode<Button>("%Reject");
    }

    public override void _ExitTree()
    {
        if (SceneStack.Current is not null && SceneStack.Current.Modal == this)
        {
            SceneStack.Current.Modal = null;
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationVisibilityChanged)
        {
            if (InputManager.Current is not null && previousFirstSelected is not null)
            {
                if (Visible)
                {
                    InputManager.Current.FirstSelected = accept;
                }
                else
                {
                    InputManager.Current.FirstSelected = previousFirstSelected;
                }
            }
        }
    }

    void IModal.Accept()
    {
        accept.EmitSignal(Button.SignalName.Confirm);
    }

    void IModal.Reject()
    {
        reject.EmitSignal(Button.SignalName.Confirm);
    }
}