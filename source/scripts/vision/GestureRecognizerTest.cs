namespace Party.Game.Detection;

public sealed partial class GestureRecognizerTest : DetectionTest<GestureRecognizerAnnotator, GestureRecognizerResult>
{
    protected override DetectionTask<GestureRecognizerResult> CreateTask()
    {
        return new GestureRecognizer();
    }
}
