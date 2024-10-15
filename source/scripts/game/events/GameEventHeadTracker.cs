using Godot;
using Party.Game.Detection;

namespace Party.Game.Experience.Events;

public abstract partial class GameEventHeadTracker : GameEventPoseLandmarker
{
    protected abstract void OnDetect(Vector2 center, float radius);

    protected sealed override void OnDetect(PoseLandmarkerResult output)
    {
        var size = Get(Control.PropertyName.Size).As<Vector2>();

        var nose = output[PoseLandmark.Nose];
        var earL = output[PoseLandmark.LeftEar];
        var earR = output[PoseLandmark.RightEar];

        var p1 = new Vector2(earL.X, earL.Y) * size;
        var p2 = new Vector2(earR.X, earR.Y) * size;

        OnDetect(new Vector2(nose.X, nose.Y) * size, p1.DistanceTo(p2));
    }
}
