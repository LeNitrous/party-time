using System;
using GDExtension.Wrappers;
using Godot;

namespace Party.Game.Detection;

public sealed class GestureRecognizer : DetectionTask<MediaPipeGestureRecognizer, MediaPipeGestureRecognizerResult, GestureRecognizerResult>
{
    public GestureRecognizer()
        : base("res://vision/gesture_recognizer.task")
    {
    }

    protected override MediaPipeGestureRecognizer Create()
    {
        var task = MediaPipeGestureRecognizer.Instantiate();
        task.ResultCallback += OnAsyncResult;
        return task;
    }

    protected override void Destroy(MediaPipeGestureRecognizer task)
    {
        task.ResultCallback -= OnAsyncResult;
    }

    protected override void Detect(MediaPipeGestureRecognizer task, MediaPipeImage image, Rect2 region, int timestamp)
    {
        task.RecognizeAsync(image, timestamp, region, 0);
    }

    protected override MediaPipeGestureRecognizerResult DetectFromImage(MediaPipeGestureRecognizer task, MediaPipeImage image, Rect2 region)
    {
        return task.Recognize(image, region, 0);
    }

    protected override MediaPipeGestureRecognizerResult DetectFromVideo(MediaPipeGestureRecognizer task, MediaPipeImage image, Rect2 region, int timestamp)
    {
        return task.RecognizeVideo(image, timestamp, region, 0);
    }

    protected override void Initialize(MediaPipeGestureRecognizer task, MediaPipeTaskBaseOptions options, int mode)
    {
        task.Initialize(options, mode, 2, 0.5f, 0.5f, 0.5f);
    }

    protected override GestureRecognizerResult Transform(MediaPipeGestureRecognizerResult output)
    {
        if (output.Gestures.Count == 0)
        {
            return default;
        }

        if (output.Handedness.Count != output.Gestures.Count)
        {
            return default;
        }

        if (output.HandLandmarks.Count != output.Gestures.Count)
        {
            return default;
        }

        var count = Mathf.Min(output.Gestures.Count, 2);

        Span<GestureRecognizerResult.Hand> hands = stackalloc GestureRecognizerResult.Hand[count];

        for (int i = 0; i < count; i++)
        {
            var boundsMin = new Vector2(float.MaxValue, float.MaxValue);
            var boundsMax = new Vector2(float.MinValue, float.MinValue);
        
            for (int j = 0; j < 21; j++)
            {
                var landmark = output.HandLandmarks[i].GetRepeated("landmark", j);
                var point = new Vector2(landmark.Get<float>("x"), landmark.Get<float>("y"));
                boundsMin = boundsMin.Min(point);
                boundsMax = boundsMax.Max(point);
            }

            var bounds = new Rect2(boundsMin, boundsMax - boundsMin);

            var gesture = output.Gestures[i].GetRepeated("classification", 0).Get<string>("label") switch
            {
                "Closed_Fist" => Gesture.Fist,
                "Open_Palm" => Gesture.Palm,
                "Pointing_Up" => Gesture.PointsUp,
                "Thumb_Up" => Gesture.ThumbsUp,
                "Thumb_Down" => Gesture.ThumbsDown,
                "ILoveYou" => Gesture.Love,
                "Victory" => Gesture.Victory,
                "None" => Gesture.None,
                _ => Gesture.Unknown,
            };

            var handedness = output.Handedness[i].GetRepeated("classification", 0).Get<string>("label") switch
            {
                "Left" => GestureHandedness.Left,
                _ => GestureHandedness.Right,
            };

            hands[i] = new GestureRecognizerResult.Hand(gesture, handedness, bounds);
        };

        return new GestureRecognizerResult(hands);
    }
}
