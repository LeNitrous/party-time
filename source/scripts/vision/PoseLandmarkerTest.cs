namespace Party.Game.Detection;

public sealed partial class PoseLandmarkerTest : DetectionTest<PoseLandmarkerAnnotator, PoseLandmarkerResult>
{
    protected override DetectionTask<PoseLandmarkerResult> CreateTask()
    {
        return new PoseLandmarker();
    }
}
