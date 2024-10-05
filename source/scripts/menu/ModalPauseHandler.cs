using Godot;

namespace Party.Game.Menu;

public sealed partial class ModalPauseHandler : ModalExitHandler
{
    protected override void OnAttach()
    {
        base.OnAttach();
        Parent.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(onVisibilityChanged));
    }

    protected override void OnDetach()
    {
        base.OnDetach();
        Parent.Disconnect(CanvasItem.SignalName.VisibilityChanged, Callable.From(onVisibilityChanged));
    }

    private void onVisibilityChanged()
    {
        GetTree().Paused = Parent.Visible;
    }
}
