using System;
using GDExtension.Wrappers;
using Godot;

namespace Party.Game.Detection;

public sealed class PoseLandmarker : DetectionTask<MediaPipePoseLandmarker, MediaPipePoseLandmarkerResult, PoseLandmarkerResult>
{
    public PoseLandmarker()
        : base("res://vision/pose_landmarker.task")
    {
    }

    protected override MediaPipePoseLandmarker Create()
    {
        var task = MediaPipePoseLandmarker.Instantiate();
        task.ResultCallback += OnAsyncResult;
        return task;
    }

    protected override void Initialize(MediaPipePoseLandmarker task, MediaPipeTaskBaseOptions options, int mode)
    {
        task.Initialize(options, mode, 1, 0.5f, 0.5f, 0.5f, false);
    }

    protected override void Destroy(MediaPipePoseLandmarker task)
    {
        task.ResultCallback -= OnAsyncResult;
    }

    protected override void Detect(MediaPipePoseLandmarker task, MediaPipeImage image, Rect2 region, int timestamp)
    {
        task.DetectAsync(image, timestamp, region, 0);
    }

    protected override MediaPipePoseLandmarkerResult DetectFromImage(MediaPipePoseLandmarker task, MediaPipeImage image, Rect2 region)
    {
        return task.Detect(image, region, 0);
    }

    protected override MediaPipePoseLandmarkerResult DetectFromVideo(MediaPipePoseLandmarker task, MediaPipeImage image, Rect2 region, int timestamp)
    {
        return task.DetectVideo(image, timestamp, region, 0);
    }

    protected override PoseLandmarkerResult Transform(MediaPipePoseLandmarkerResult output)
    {
        if (output.PoseLandmarks.Count <= 0)
        {
            return default;
        }

        Span<Vector3> span = stackalloc Vector3[(int)PoseLandmark.Maximum];

        for (int i = 0; i < span.Length; i++)
        {
            var landmark = output.PoseLandmarks[0].Landmarks[i];
            span[i] = new Vector3(landmark.X, landmark.Y, landmark.Z);
        }

        return new PoseLandmarkerResult(span);
    }
}
