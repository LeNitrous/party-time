namespace Party.Game.Experience.Events;

public sealed partial class GameEventTutorial_02 : GameEvent
{
    public override double Duration => 30.0;

    private static readonly string[] dialogue =
    [
        "GAME_TUTORIAL_02_1",
        "GAME_TUTORIAL_02_2",
        "GAME_TUTORIAL_02_3",
    ];

    public override void _Ready()
    {
        var help = GetNode<GameEventTutorialHelper>("Help");
        help.OnFinish += () => Trigger(Completion.Win);
        help.Start(dialogue);
    }
}
