using System;
using Godot;
using Party.Game.Vision;

namespace Party.Game.Testing;

public sealed partial class DetectionAnnotator : Control
{
    public override void _Ready()
    {
        if (VisionService.Current is not null)
        {
            VisionService.Current.SourceChanged += onSourceChanged;
        }
    }

    public override void _ExitTree()
    {
        if (VisionService.Current is not null)
        {
            VisionService.Current.SourceChanged -= onSourceChanged;
        }
    }

    private void onSourceChanged(IFrameSource prev, IFrameSource next)
    {
        if (prev is not null)
        {
            prev.OnFrame -= onFrame;
        }

        if (next is not null)
        {
            next.OnFrame += onFrame;
        }
    }

    private void onFrame(Image image)
    {
        CallDeferred(MethodName.QueueRedraw);
    }

    public override void _Draw()
    {
        if (VisionService.Current is null)
        {
            return;
        }

        if (!VisionService.Current.IsTracking)
        {
            return;
        }

        Span<Vector3> landmarks = stackalloc Vector3[(int)VisionLandmark.Maximum];
        VisionService.Current.GetLandmarks(landmarks);

        foreach (var landmark in landmarks)
        {
            DrawCircle(new Vector2(landmark.X, landmark.Y) * Size, 2.0f, Colors.Green, true, -1, true);
        }

        foreach (var connection in connections)
        {
            var a = landmarks[(int)connection.Item1];
            var b = landmarks[(int)connection.Item2];
            DrawLine(new Vector2(a.X, a.Y) * Size, new Vector2(b.X, b.Y) * Size, Colors.Green, 2.0f, true);
        }
    }

    private static readonly (VisionLandmark, VisionLandmark)[] connections =
    [
        // FACE
        (VisionLandmark.RightEar, VisionLandmark.RightEye),
        (VisionLandmark.RightEye, VisionLandmark.Nose),
        (VisionLandmark.LeftEar, VisionLandmark.LeftEye),
        (VisionLandmark.LeftEye, VisionLandmark.Nose),
        (VisionLandmark.LeftMouth, VisionLandmark.RightMouth),

        // TORSO
        (VisionLandmark.LeftShoulder, VisionLandmark.RightShoulder),
        (VisionLandmark.LeftShoulder, VisionLandmark.LeftHip),
        (VisionLandmark.RightShoulder, VisionLandmark.RightHip),
        (VisionLandmark.LeftHip, VisionLandmark.RightHip),

        // LEFT ARM
        (VisionLandmark.LeftShoulder, VisionLandmark.LeftElbow),
        (VisionLandmark.LeftElbow, VisionLandmark.LeftWrist),
        (VisionLandmark.LeftWrist, VisionLandmark.LeftThumb),
        (VisionLandmark.LeftWrist, VisionLandmark.LeftIndex),
        (VisionLandmark.LeftWrist, VisionLandmark.LeftPinky),
        (VisionLandmark.LeftPinky, VisionLandmark.LeftIndex),

        // LEFT LEG
        (VisionLandmark.LeftHip, VisionLandmark.LeftKnee),
        (VisionLandmark.LeftKnee, VisionLandmark.LeftAnkle),
        (VisionLandmark.LeftAnkle, VisionLandmark.LeftHeel),
        (VisionLandmark.LeftAnkle, VisionLandmark.LeftToe),
        (VisionLandmark.LeftHeel, VisionLandmark.LeftToe),

        // RIGHT ARM
        (VisionLandmark.RightShoulder, VisionLandmark.RightElbow),
        (VisionLandmark.RightElbow, VisionLandmark.RightWrist),
        (VisionLandmark.RightWrist, VisionLandmark.RightThumb),
        (VisionLandmark.RightWrist, VisionLandmark.RightIndex),
        (VisionLandmark.RightWrist, VisionLandmark.RightPinky),
        (VisionLandmark.RightPinky, VisionLandmark.RightIndex),

        // RIGHT LEG
        (VisionLandmark.RightHip, VisionLandmark.RightKnee),
        (VisionLandmark.RightKnee, VisionLandmark.RightAnkle),
        (VisionLandmark.RightAnkle, VisionLandmark.RightHeel),
        (VisionLandmark.RightAnkle, VisionLandmark.RightToe),
        (VisionLandmark.RightHeel, VisionLandmark.RightToe),
    ];
}
