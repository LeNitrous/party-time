namespace Party.Game.Menu;

public sealed partial class PromptExitHandler : Component<Prompt>
{
    protected override void OnAttach()
    {
    }

    private void handleExitRequest()
    {
        SceneStack.Current?.Exit();
    }
}