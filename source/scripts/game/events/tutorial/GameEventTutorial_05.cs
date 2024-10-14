namespace Party.Game.Experience.Events;

public sealed partial class GameEventTutorial_05 : GameEvent
{
    private static readonly string[] dialogue =
    [
        "Congratulations on completing the tutorial.",
        "Now go ahead and play to experience what this game has to offer!",
    ];

    public override void _Ready()
    {
        var help = GetNode<GameEventTutorialHelper>("Help");
        help.OnFinish += () => Trigger(Completion.Win);
        help.Start(dialogue);
    }
}