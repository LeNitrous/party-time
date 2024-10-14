using Godot;

namespace Party.Game.Menu;

public sealed partial class ButtonSceneStackPush : ButtonConfirmHandler
{
    [Export(PropertyHint.File, "*.tscn")]
    public string Scene { get; set; }

    [Export]
    public bool Append { get; set; } = true;

    protected override void OnConfirm()
    {
        if (SceneStack.Current is null)
        {
            return;
        }

        SceneStack.Current.Push(Scene, Append);
    }
}
