namespace Party.Game.Experience.Events;

public sealed partial class GameEventTutorial_01 : GameEvent
{
    private static readonly string[] dialogue =
    [
        "GAME_TUTORIAL_01_1",
        "GAME_TUTORIAL_01_2",
        "GAME_TUTORIAL_01_3",
        "GAME_TUTORIAL_01_4",
        "GAME_TUTORIAL_01_5",
    ];

    public override void _Ready()
    {
        var help = GetNode<GameEventTutorialHelper>("Help");
        help.OnFinish += () => Trigger(Completion.Win);
        help.Start(dialogue);
    }
}
