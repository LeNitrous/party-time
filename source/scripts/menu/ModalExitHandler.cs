namespace Party.Game.Menu;

public partial class ModalExitHandler : ModalSignalHandler
{
    public override void _Ready()
    {
        if (SceneStack.Current is not null)
        {
            SceneStack.Current.Modal = Parent;
        }
    }

    public override void _ExitTree()
    {
        if (SceneStack.Current is not null && SceneStack.Current.Modal == Parent)
        {
            SceneStack.Current.Modal = null;
        }
    }

    protected override void OnAccept()
    {
        SceneStack.Current?.Exit();
    }

    protected override void OnReject()
    {
        Parent.Hide();
    }
}
