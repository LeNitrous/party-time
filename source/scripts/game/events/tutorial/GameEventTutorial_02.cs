namespace Party.Game.Experience.Events;

public sealed partial class GameEventTutorial_02 : GameEvent
{
    public override double Duration => 30.0;

    private static readonly string[] dialogue =
    [
        "Some events are timed as you can see in the bottom right corner.",
        "Make sure you finish the objectives before the timer runs out!",
        "But since you've been paying attention, you win this event again!",
    ];

    public override void _Ready()
    {
        var help = GetNode<GameEventTutorialHelper>("Help");
        help.OnFinish += () => Trigger(Completion.Win);
        help.Start(dialogue);
    }
}
