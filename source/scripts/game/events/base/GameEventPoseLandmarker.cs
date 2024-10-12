using Party.Game.Detection;

namespace Party.Game.Experience.Events;

public abstract partial class GameEventPoseLandmarker : GameEventDetectionTask<PoseLandmarkerResult>
{
    protected sealed override DetectionTask<PoseLandmarkerResult> CreateTask()
    {
        return new PoseLandmarker();
    }

    protected sealed override DetectionAnnotator<PoseLandmarkerResult> CreateAnnotator()
    {
        return new PoseLandmarkerAnnotator();
    }

    protected sealed override bool IsDetectionValid(PoseLandmarkerResult output)
    {
        return output.IsValid;
    }
}