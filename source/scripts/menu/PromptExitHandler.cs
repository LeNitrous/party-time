namespace Party.Game.Menu;

public sealed partial class PromptExitHandler : Component<Prompt>
{
    protected override void OnAttach()
    {
        Parent.Accept += handleExitRequest;
    }

    private void handleExitRequest()
    {
        SceneStack.Current?.Exit();
    }
}