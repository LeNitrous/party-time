namespace Party.Game.Experience.Events;

public sealed partial class GameEventTutorial_05 : GameEvent
{
    private static readonly string[] dialogue =
    [
        "GAME_TUTORIAL_05_1",
        "GAME_TUTORIAL_05_2",
    ];

    public override void _Ready()
    {
        var help = GetNode<GameEventTutorialHelper>("Help");
        help.OnFinish += () => Trigger(Completion.Win);
        help.Start(dialogue);
    }
}