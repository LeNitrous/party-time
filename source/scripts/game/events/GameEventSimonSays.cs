using System.Collections.Generic;
using System.Linq;
using Godot;
using Party.Game.Detection;

namespace Party.Game.Experience.Events;

public sealed partial class GameEventSimonSays : GameEventGestureRecognizer
{
    public override double Duration => 10.0;

    private Gesture gesture;

    protected override void Init()
    {
        GetNode<Label>("Label").Text = Tr(gestureDescription[gesture = possible.GetRandom()]);
    }

    protected override void OnDetect(GestureRecognizerResult output)
    {
        if (output.Any(h => h.Gesture == gesture))
        {
            Trigger(Completion.Win);
        }
    }

    private static Dictionary<Gesture, string> gestureDescription = new Dictionary<Gesture, string>
    {
        [Gesture.Love]       = "GAME_HINT_SIMONSAYS_LOVE",
        [Gesture.PointsUp]   = "GAME_HINT_SIMONSAYS_POINTSUP",
        [Gesture.ThumbsUp]   = "GAME_HINT_SIMONSAYS_THUMBSUP",
        [Gesture.Victory]    = "GAME_HINT_SIMONSAYS_VICTORY",
        [Gesture.ThumbsDown] = "GAME_HINT_SIMONSAYS_THUMBSDOWN",
    };

    private static readonly Gesture[] possible =
    [
        Gesture.Love,
        Gesture.PointsUp,
        Gesture.ThumbsUp,
        Gesture.Victory,
        Gesture.ThumbsDown,
    ];
}