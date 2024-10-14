using System;
using Godot;

namespace Party.Game.Experience.Events;

public sealed partial class GameEventTutorialHelper : Node
{
    public event Action OnFinish;

    private string[] dialogue = Array.Empty<string>();
    private Timer timer;
    private Label label;
    private int current;

    public override void _Ready()
    {
        label = GetParent().GetNode<Label>("Label");
        timer = GetParent().GetNode<Timer>("Timer");
        timer.Autostart = false;
        timer.OneShot = false;
        timer.Timeout += onTimeout;
    }

    public void Start(string[] lines)
    {
        current = -1;
        dialogue = lines;
        timer.WaitTime = 3.0;
        timer.Start();
        onTimeout();
    }

    private void onTimeout()
    {
        int next = current + 1;

        if (next >= dialogue.Length)
        {
            timer.Stop();
            OnFinish?.Invoke();
        }
        else
        {
            label.Text = dialogue[current = next];
        }
    }
}
