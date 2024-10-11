using Godot;

namespace Party.Game.Detection;

public sealed partial class PoseLandmarkerAnnotator : DetectionAnnotator<PoseLandmarkerResult>
{
    protected override void Render(PoseLandmarkerResult output)
    {
        if (!output.IsValid)
        {
            return;
        }

        foreach (var point in output)
        {
            DrawCircle(new Vector2(point.X, point.Y) * Size, 1.0f, Colors.Green, true, -1, true);
        }

        foreach ((var a, var b) in connections)
        {
            DrawLine(new Vector2(output[a].X, output[a].Y) * Size, new Vector2(output[b].X, output[b].Y) * Size, Colors.Green, -1, true);
        }
    }

    private static readonly (PoseLandmark, PoseLandmark)[] connections =
    [
        // FACE
        (PoseLandmark.RightEar, PoseLandmark.RightEye),
        (PoseLandmark.RightEye, PoseLandmark.Nose),
        (PoseLandmark.LeftEar, PoseLandmark.LeftEye),
        (PoseLandmark.LeftEye, PoseLandmark.Nose),
        (PoseLandmark.LeftMouth, PoseLandmark.RightMouth),

        // TORSO
        (PoseLandmark.LeftShoulder, PoseLandmark.RightShoulder),
        (PoseLandmark.LeftShoulder, PoseLandmark.LeftHip),
        (PoseLandmark.RightShoulder, PoseLandmark.RightHip),
        (PoseLandmark.LeftHip, PoseLandmark.RightHip),

        // LEFT ARM
        (PoseLandmark.LeftShoulder, PoseLandmark.LeftElbow),
        (PoseLandmark.LeftElbow, PoseLandmark.LeftWrist),
        (PoseLandmark.LeftWrist, PoseLandmark.LeftThumb),
        (PoseLandmark.LeftWrist, PoseLandmark.LeftIndex),
        (PoseLandmark.LeftWrist, PoseLandmark.LeftPinky),
        (PoseLandmark.LeftPinky, PoseLandmark.LeftIndex),

        // LEFT LEG
        (PoseLandmark.LeftHip, PoseLandmark.LeftKnee),
        (PoseLandmark.LeftKnee, PoseLandmark.LeftAnkle),
        (PoseLandmark.LeftAnkle, PoseLandmark.LeftHeel),
        (PoseLandmark.LeftAnkle, PoseLandmark.LeftToe),
        (PoseLandmark.LeftHeel, PoseLandmark.LeftToe),

        // RIGHT ARM
        (PoseLandmark.RightShoulder, PoseLandmark.RightElbow),
        (PoseLandmark.RightElbow, PoseLandmark.RightWrist),
        (PoseLandmark.RightWrist, PoseLandmark.RightThumb),
        (PoseLandmark.RightWrist, PoseLandmark.RightIndex),
        (PoseLandmark.RightWrist, PoseLandmark.RightPinky),
        (PoseLandmark.RightPinky, PoseLandmark.RightIndex),

        // RIGHT LEG
        (PoseLandmark.RightHip, PoseLandmark.RightKnee),
        (PoseLandmark.RightKnee, PoseLandmark.RightAnkle),
        (PoseLandmark.RightAnkle, PoseLandmark.RightHeel),
        (PoseLandmark.RightAnkle, PoseLandmark.RightToe),
        (PoseLandmark.RightHeel, PoseLandmark.RightToe),
    ];
}
