using System.Linq;
using Party.Game.Detection;

namespace Party.Game.Experience.Events;

public sealed partial class GameEventTutorial_03 : GameEventGestureRecognizer
{
    private static readonly string[] dialogue =
    [
        "Many of the instructions in this game require you to perform an action.",
        "Lets keep things simple. If you've been paying attention so far, give me a thumbs up.",
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
