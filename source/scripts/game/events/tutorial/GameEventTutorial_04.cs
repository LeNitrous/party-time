using System.Linq;
using Party.Game.Detection;

namespace Party.Game.Experience.Events;

public sealed partial class GameEventTutorial_04 : GameEventGestureRecognizer
{
    public override double Duration => 40.0;

    private static readonly string[] dialogue =
    [
        "Great job! Looks like you're getting the hang of this now.",
        "Let's make this more challenging for you.",
        "Give me a thumbs up but this time with a timer."
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
