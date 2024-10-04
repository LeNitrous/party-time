using Godot;

namespace Party.Game.Menu;

public sealed partial class ButtonSceneStackPush : ButtonConfirmHandler
{
    [Export(PropertyHint.File, "*.tscn")]
    public string Scene { get; set; }

    protected override void OnConfirm()
    {
        if (SceneStack.Current is null)
        {
            return;
        }

        SceneStack.Current.Push(Scene);
    }
}
