using Godot;
using Party.Game.Detection;

namespace Party.Game.Experience.Events;

public abstract partial class GameEventHandTracker : GameEventPoseLandmarker
{
    protected abstract void OnDetect(GestureHandedness hand, Vector2 center, float radius);

    protected sealed override void OnDetect(PoseLandmarkerResult output)
    {
        static void getCircle(float x1, float y1, float x2, float y2, float x3, float y3, out Vector2 center, out float radius)
        {
            float x12 = x1 - x2;
            float x13 = x1 - x3;

            float y12 = y1 - y2;
            float y13 = y1 - y3;

            float y31 = y3 - y1;
            float y21 = y2 - y1;

            float x31 = x3 - x1;
            float x21 = x2 - x1;

            float sx13 = Mathf.Pow(x1, 2) - Mathf.Pow(x3, 2);
            float sy13 = Mathf.Pow(y1, 2) - Mathf.Pow(y3, 2);

            float sx21 = Mathf.Pow(x2, 2) - Mathf.Pow(x1, 2);
            float sy21 = Mathf.Pow(y2, 2) - Mathf.Pow(y1, 2);

            float f = ((sx13 * x12) + (sy13 * x12) + (sx21 * x13) + (sy21 * x13)) / (2 * (y31 * x12) - (y21 * x13));
            float g = ((sx13 * y12) + (sy13 * y12) + (sx21 * y13) + (sy21 * y13)) / (2 * (x31 * y12) - (x21 * y13));
            float c = Mathf.Pow(x1, 2) - Mathf.Pow(y1, 2) - 2 * g * x1 - 2 * f * y1;

            float h = -g;
            float k = -f;

            center = new Vector2(h, k);
            radius = Mathf.Sqrt(h * h + k * k - c);
        }

        var size = Call(Control.PropertyName.Size).As<Vector2>();

        var indexL = output[PoseLandmark.LeftIndex] * new Vector3(size.X, size.Y, 1);
        var pinkyL = output[PoseLandmark.LeftPinky] * new Vector3(size.X, size.Y, 1);
        var wristL = output[PoseLandmark.LeftWrist] * new Vector3(size.X, size.Y, 1);

        getCircle(indexL.X, indexL.Y, pinkyL.X, pinkyL.Y, wristL.X, wristL.Y, out var centerL, out float radiusL);
        OnDetect(GestureHandedness.Left, centerL, radiusL);

        var indexR = output[PoseLandmark.RightIndex] * new Vector3(size.X, size.Y, 1);
        var pinkyR = output[PoseLandmark.RightPinky] * new Vector3(size.X, size.Y, 1);
        var wristR = output[PoseLandmark.RightWrist] * new Vector3(size.X, size.Y, 1);

        getCircle(indexR.X, indexR.Y, pinkyR.X, pinkyR.Y, wristR.X, wristR.Y, out var centerR, out float radiusR);
        OnDetect(GestureHandedness.Right, centerR, radiusR);
    }
}
