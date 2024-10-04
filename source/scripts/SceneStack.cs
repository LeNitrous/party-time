using System;
using System.Collections.Generic;
using Godot;

namespace Party.Game;

public partial class SceneStack : Node
{
    public static SceneStack Current { get; private set; }

    public bool IsRoot => scenes.Count == 0;

    private readonly Stack<string> scenes = new Stack<string>();

    public override void _Ready()
    {
        if (Current is null)
        {
            Current = this;
            GetTree().AutoAcceptQuit = false;
            GetTree().QuitOnGoBack = false;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public override void _Notification(int what)
    {
        switch (what)
        {
            case (int)NotificationWMCloseRequest:
            case (int)NotificationWMGoBackRequest:
                Exit();
                break;
        }
    }

    public override void _ExitTree()
    {
        if (Current is not null && Current == this)
        {
            Current = null;
        }
    }

    public void Push(string path)
    {
        scenes.Push(GetTree().CurrentScene.SceneFilePath);
        CallDeferred(MethodName.changeSceneToFile, path);
    }

    public void Exit()
    {
        if (scenes.TryPop(out string path))
        {
            CallDeferred(MethodName.changeSceneToFile, path);
        }
        else if (OS.HasFeature("editor"))
        {
            GetTree().Quit();
        }
        else
        {
            GD.PrintErr(nameof(SceneStack), " :: Attempted to exit scene but there are no scenes left!");
        }
    }

    private void changeSceneToFile(string path)
    {
        var result = GetTree().ChangeSceneToFile(path);

        if (result != Error.Ok)
        {
            GD.PrintErr(result);
        }
        else
        {
            GD.Print(nameof(SceneStack), " :: change(", path, ")");
        }
    }
}
