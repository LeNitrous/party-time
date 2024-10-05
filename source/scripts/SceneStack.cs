using System;
using System.Collections.Generic;
using Godot;
using Party.Game.Menu;

namespace Party.Game;

public partial class SceneStack : Node
{
    public static SceneStack Current { get; private set; }

    public bool IsRoot => scenes.Count == 0;

    public Modal Modal { get; set; }

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

    public override void _Input(InputEvent e)
    {
        if (e.IsActionReleased("ui_cancel"))
        {
            handleQuitRequest();
        }
    }

    public override void _Notification(int what)
    {
        if (what is ((int)NotificationWMCloseRequest) or ((int)NotificationWMGoBackRequest))
        {
            handleQuitRequest();
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
        else
        {
            GetTree().Quit();
        }
    }

    private void handleQuitRequest()
    {
        if (Modal is not null)
        {
            if (Modal.Visible)
            {
                Modal.EmitSignal(Modal.SignalName.Accept);
            }
            else
            {
                Modal.Show();
            }
        }
        else
        {
            Exit();
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
