using Party.Game.Detection;

namespace Party.Game.Experience.Events;

public abstract partial class GameEventGestureRecognizer : GameEventDetectionTask<GestureRecognizerResult>
{
    protected sealed override DetectionTask<GestureRecognizerResult> CreateTask()
    {
        return new GestureRecognizer();
    }

    protected sealed override DetectionAnnotator<GestureRecognizerResult> CreateAnnotator()
    {
        return new GestureRecognizerAnnotator();
    }

    protected sealed override bool IsDetectionValid(GestureRecognizerResult output)
    {
        return output.Count > 0;
    }
}
