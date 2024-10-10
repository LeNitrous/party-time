using System;
using System.Collections.Generic;
using System.Diagnostics;
using GDExtension.Wrappers;
using Godot;

namespace Party.Game.Vision;

public sealed partial class VisionService : Node
{
    public static VisionService Current { get; private set; }

    public IFrameSource Source
    {
        get => source;
        set
        {
            if (ReferenceEquals(source, value))
            {
                return;
            }

            onSourceChanged(ref source, value);
        }
    }

    public event Action<IFrameSource, IFrameSource> SourceChanged;

    public bool IsTracking { get; private set; }

    private IFrameSource source;
    private MediaPipeImage mp;
    private MediaPipePoseLandmarker task;
    private readonly Vector3[] landmarks = new Vector3[(int)VisionLandmark.Maximum];

    public override void _Ready()
    {
        if (Current is null)
        {
            Current = this;
        }

        initialize();
    }

    public override void _ExitTree()
    {
        if (Current == this)
        {
            Current = null;
        }

        if (task is not null)
        {
            task.ResultCallback -= onResult;
            task.Dispose();
        }

        mp?.Dispose();
    }

    public void GetLandmarks(Span<Vector3> span)
    {
        landmarks.CopyTo(span);
    }

    public Vector3 GetLandmark(VisionLandmark landmark)
    {
        return landmarks[(int)landmark];
    }

    private void onSourceChanged(ref IFrameSource prev, IFrameSource next)
    {
        if (prev is not null)
        {
            prev.OnFrame -= onFrame;
        }

        if (next is not null)
        {
            next.OnFrame += onFrame;
        }

        SourceChanged?.Invoke(prev, next);

        prev = next;
        initialize();
    }

    private void initialize()
    {
        if (task is not null)
        {
            task.ResultCallback -= onResult;
            task.Dispose();
            task = null;
        }

        if (source is null)
        {
            return;
        }

        var file = FileAccess.Open("res://vision/pose_landmarker.task", FileAccess.ModeFlags.Read);
        
        if (file is null)
        {
            GD.PrintErr(FileAccess.GetOpenError());
            return;
        }

        using var options = MediaPipeTaskBaseOptions.Instantiate();
        options.Delegate = (int)(source.Accelerated ? MediaPipeTaskBaseOptions.DelegateEnum.DelegateGpu : MediaPipeTaskBaseOptions.DelegateEnum.DelegateCpu);
        options.ModelAssetBuffer = file.GetBuffer((long)file.GetLength());

        int running = source.Kind switch
        {
            FrameSourceKind.Image => (int)MediaPipeTask.VisionRunningMode.RunningModeImage,
            FrameSourceKind.Video => (int)MediaPipeTask.VisionRunningMode.RunnineModeVideo,
            _ => (int)MediaPipeTask.VisionRunningMode.RunningModeLiveStream,
        };

        task = MediaPipePoseLandmarker.Instantiate();
        task.ResultCallback += onResult;
        task.Initialize(options, running, 1, 0.5f, 0.5f, 0.5f, false);
    }

    private void onFrame(Image image)
    {
        mp ??= MediaPipeImage.Instantiate();
        mp.SetImage(image);

        int time = (int)Time.GetTicksMsec();
        var area = new Rect2I(Vector2I.Zero, image.GetSize());

        if (source.Kind is FrameSourceKind.Image)
        {
            onResult(task.Detect(mp, area, 0), mp, time);
        }
        
        if (source.Kind is FrameSourceKind.Video)
        {
            onResult(task.DetectVideo(mp, time, area, 0), mp, time);
        }

        if (source.Kind is FrameSourceKind.Stream)
        {
            task.DetectAsync(mp, time, area, 0);
        }
    }

    private void onResult(MediaPipePoseLandmarkerResult result, MediaPipeImage image, int timestamp)
    {
        if (result.PoseLandmarks.Count <= 0)
        {
            for (int i = 0; i < landmarks.Length; i++)
            {
                landmarks[i] = Vector3.Zero;
            }

            IsTracking = false;
        }
        else
        {
            var pose = result.PoseLandmarks[0];

            for (int i = 0; i < landmarks.Length; i++)
            {
                landmarks[i] = new Vector3(pose.Landmarks[i].X, pose.Landmarks[i].Y, pose.Landmarks[i].Z);
            }

            IsTracking = true;
        }
    }
}
