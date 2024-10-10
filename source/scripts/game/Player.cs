using System;
using Godot;
using Party.Game.Vision;

namespace Party.Game.Experience;

public sealed partial class Player : Area2D
{
    private CollisionShape2D head;
    private CollisionPolygon2D body;
    private readonly CollisionShape2D[] joints = new CollisionShape2D[points.Length];
    private readonly CollisionShape2D[] segments = new CollisionShape2D[connections.Length];

    public override void _Ready()
    {
        head = GetNode<CollisionShape2D>("Head");
        body = GetNode<CollisionPolygon2D>("Body");
    }

    public override void _Process(double delta)
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

        for (int i = 0; i < connections.Length; i++)
        {
            if (segments[i] is null)
            {
                segments[i] = new CollisionShape2D();
                segments[i].Shape = new SegmentShape2D();
                AddChild(segments[i]);
            }

            var a = landmarks[(int)connections[i].Item1];
            var b = landmarks[(int)connections[i].Item2];

            ((SegmentShape2D)segments[i].Shape).A = new Vector2(a.X, a.Y) * size;
            ((SegmentShape2D)segments[i].Shape).B = new Vector2(b.X, b.Y) * size;
        }

        for (int i = 0; i < points.Length; i++)
        {
            if (joints[i] is null)
            {
                joints[i] = new CollisionShape2D();
                joints[i].Shape = new CircleShape2D();
                AddChild(joints[i]);
            }

            var point = landmarks[(int)points[i]];
            joints[i].Position = new Vector2(point.X, point.Y) * size;
        }

        var nose = landmarks[(int)VisionLandmark.Nose];
        head.Position = new Vector2(nose.X, nose.Y) * size;

        var shoulderR = landmarks[(int)VisionLandmark.RightShoulder];
        var hipR = landmarks[(int)VisionLandmark.RightHip];
        var hipL = landmarks[(int)VisionLandmark.LeftHip];
        var shoulderL = landmarks[(int)VisionLandmark.LeftShoulder];

        body.Polygon =
        [
            new Vector2(shoulderL.X, shoulderL.Y) * size,
            new Vector2(hipL.X, hipL.Y) * size,
            new Vector2(hipR.X, hipR.Y) * size,
            new Vector2(shoulderR.X, shoulderR.Y) * size,
        ];
    }

    private static readonly Vector2 size = new Vector2(1280, 720);

    private static readonly VisionLandmark[] torso =
    [
        VisionLandmark.RightShoulder,
        VisionLandmark.RightHip,
        VisionLandmark.LeftHip,
        VisionLandmark.LeftShoulder,
    ];

    private static readonly VisionLandmark[] points =
    [
        VisionLandmark.LeftShoulder,
        VisionLandmark.LeftElbow,
        VisionLandmark.LeftWrist,
        VisionLandmark.LeftHip,
        VisionLandmark.LeftKnee,
        VisionLandmark.LeftHeel,
        VisionLandmark.RightShoulder,
        VisionLandmark.RightElbow,
        VisionLandmark.RightWrist,
        VisionLandmark.RightHip,
        VisionLandmark.RightKnee,
        VisionLandmark.RightHeel,
    ];

    private static readonly (VisionLandmark, VisionLandmark)[] connections =
    [
        // LEFT ARM
        (VisionLandmark.LeftShoulder, VisionLandmark.LeftElbow),
        (VisionLandmark.LeftElbow, VisionLandmark.LeftWrist),

        // LEFT LEG
        (VisionLandmark.LeftHip, VisionLandmark.LeftKnee),
        (VisionLandmark.LeftKnee, VisionLandmark.LeftAnkle),

        // RIGHT ARM
        (VisionLandmark.RightShoulder, VisionLandmark.RightElbow),
        (VisionLandmark.RightElbow, VisionLandmark.RightWrist),

        // RIGHT LEG
        (VisionLandmark.RightHip, VisionLandmark.RightKnee),
        (VisionLandmark.RightKnee, VisionLandmark.RightAnkle),
    ];
}