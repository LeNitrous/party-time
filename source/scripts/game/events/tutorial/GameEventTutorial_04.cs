using System.Linq;
using Party.Game.Detection;

namespace Party.Game.Experience.Events;

public sealed partial class GameEventTutorial_04 : GameEventGestureRecognizer
{
    public override double Duration => 40.0;

    private static readonly string[] dialogue =
    [
        "GAME_TUTORIAL_04_1",
        "GAME_TUTORIAL_04_2",
        "GAME_TUTORIAL_04_3",
    ];

    private bool isDialogueFinished;

    public override void _Ready()
    {
        base._Ready();
        var help = GetNode<GameEventTutorialHelper>("Help");
        help.OnFinish += () => isDialogueFinished = true;
        help.Start(dialogue);
    }

    protected override void OnDetect(GestureRecognizerResult output)
    {
        if (!isDialogueFinished)
        {
            return;
        }

        if (output.Any(h => h.Gesture == Gesture.ThumbsUp))
        {
            Trigger(Completion.Win);
        }
    }
}
