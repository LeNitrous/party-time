using Godot;

namespace Party.Game.Experience;

public sealed partial class Player : Area2D
{
    // private CollisionShape2D head;
    // private CollisionPolygon2D body;
    // private readonly CollisionShape2D[] joints = new CollisionShape2D[points.Length];
    // private readonly CollisionShape2D[] segments = new CollisionShape2D[connections.Length];

    // public override void _Ready()
    // {
    //     head = GetNode<CollisionShape2D>("Head");
    //     body = GetNode<CollisionPolygon2D>("Body");
    // }

    // public override void _Process(double delta)
    // {
    //     if (VisionService.Current is null)
    //     {
    //         return;
    //     }

    //     if (!VisionService.Current.IsTracking)
    //     {
    //         return;
    //     }

    //     Span<Vector3> landmarks = stackalloc Vector3[(int)PoseLandmark.Maximum];
    //     VisionService.Current.GetLandmarks(landmarks);

    //     for (int i = 0; i < connections.Length; i++)
    //     {
    //         if (segments[i] is null)
    //         {
    //             segments[i] = new CollisionShape2D();
    //             segments[i].Shape = new SegmentShape2D();
    //             AddChild(segments[i]);
    //         }

    //         var a = landmarks[(int)connections[i].Item1];
    //         var b = landmarks[(int)connections[i].Item2];

    //         ((SegmentShape2D)segments[i].Shape).A = new Vector2(a.X, a.Y) * size;
    //         ((SegmentShape2D)segments[i].Shape).B = new Vector2(b.X, b.Y) * size;
    //     }

    //     for (int i = 0; i < points.Length; i++)
    //     {
    //         if (joints[i] is null)
    //         {
    //             joints[i] = new CollisionShape2D();
    //             joints[i].Shape = new CircleShape2D();
    //             AddChild(joints[i]);
    //         }

    //         var point = landmarks[(int)points[i]];
    //         joints[i].Position = new Vector2(point.X, point.Y) * size;
    //     }

    //     var nose = landmarks[(int)PoseLandmark.Nose];
    //     head.Position = new Vector2(nose.X, nose.Y) * size;

    //     var shoulderR = landmarks[(int)PoseLandmark.RightShoulder];
    //     var hipR = landmarks[(int)PoseLandmark.RightHip];
    //     var hipL = landmarks[(int)PoseLandmark.LeftHip];
    //     var shoulderL = landmarks[(int)PoseLandmark.LeftShoulder];

    //     body.Polygon =
    //     [
    //         new Vector2(shoulderL.X, shoulderL.Y) * size,
    //         new Vector2(hipL.X, hipL.Y) * size,
    //         new Vector2(hipR.X, hipR.Y) * size,
    //         new Vector2(shoulderR.X, shoulderR.Y) * size,
    //     ];
    // }

    // private static readonly Vector2 size = new Vector2(1280, 720);

    // private static readonly PoseLandmark[] torso =
    // [
    //     PoseLandmark.RightShoulder,
    //     PoseLandmark.RightHip,
    //     PoseLandmark.LeftHip,
    //     PoseLandmark.LeftShoulder,
    // ];

    // private static readonly PoseLandmark[] points =
    // [
    //     PoseLandmark.LeftShoulder,
    //     PoseLandmark.LeftElbow,
    //     PoseLandmark.LeftWrist,
    //     PoseLandmark.LeftHip,
    //     PoseLandmark.LeftKnee,
    //     PoseLandmark.LeftHeel,
    //     PoseLandmark.RightShoulder,
    //     PoseLandmark.RightElbow,
    //     PoseLandmark.RightWrist,
    //     PoseLandmark.RightHip,
    //     PoseLandmark.RightKnee,
    //     PoseLandmark.RightHeel,
    // ];

    // private static readonly (PoseLandmark, PoseLandmark)[] connections =
    // [
    //     // LEFT ARM
    //     (PoseLandmark.LeftShoulder, PoseLandmark.LeftElbow),
    //     (PoseLandmark.LeftElbow, PoseLandmark.LeftWrist),

    //     // LEFT LEG
    //     (PoseLandmark.LeftHip, PoseLandmark.LeftKnee),
    //     (PoseLandmark.LeftKnee, PoseLandmark.LeftAnkle),

    //     // RIGHT ARM
    //     (PoseLandmark.RightShoulder, PoseLandmark.RightElbow),
    //     (PoseLandmark.RightElbow, PoseLandmark.RightWrist),

    //     // RIGHT LEG
    //     (PoseLandmark.RightHip, PoseLandmark.RightKnee),
    //     (PoseLandmark.RightKnee, PoseLandmark.RightAnkle),
    // ];
}