namespace Party.Game.Experience.Events;

public sealed partial class GameEventTutorial_01 : GameEvent
{
    private static readonly string[] dialogue =
    [
        "Welcome to Party Time!",
        "The objective is really simple.",
        "Pay attention and follow instructions!",
        "Following the instructions will reward you points!",
        "Like this!"
    ];

    public override void _Ready()
    {
        var help = GetNode<GameEventTutorialHelper>("Help");
        help.OnFinish += () => Trigger(Completion.Win);
        help.Start(dialogue);
    }
}
