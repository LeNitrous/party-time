using System;
using System.Threading;
using GDExtension.Wrappers;
using Godot;

namespace Party.Game.Detection;

public abstract class DetectionTask : IDisposable
{
    public abstract void Dispose();
}

public abstract class DetectionTask<TOutput> : DetectionTask
{
    public abstract TOutput Detect(MediaPipeImage image, FrameSource sourceKind);
}

public abstract class DetectionTask<TTask, TTaskOutput, TOutput> : DetectionTask<TOutput>
    where TTask : MediaPipeTask
    where TTaskOutput : RefCounted
{
    private TTask task;
    private TTaskOutput output;
    private FrameSource source;
    private ManualResetEventSlim reset;
    private readonly string taskFilePath;

    protected DetectionTask(string taskFilePath)
    {
        this.taskFilePath = taskFilePath;
    }

    public sealed override TOutput Detect(MediaPipeImage image, FrameSource source)
    {
        if (task is null || this.source != source)
        {
            if (task is not null)
            {
                Destroy(task);
                task.Dispose();
            }

            using var file = FileAccess.Open(taskFilePath, FileAccess.ModeFlags.Read);
            using var opts = MediaPipeTaskBaseOptions.Instantiate();
            opts.Delegate = 
#if GODOT_MOBILE || GODOT_LINUXBSD
                (int)MediaPipeTaskBaseOptions.DelegateEnum.DelegateGpu;
#else
                (int)MediaPipeTaskBaseOptions.DelegateEnum.DelegateCpu;
#endif
            opts.ModelAssetBuffer = file.GetBuffer((long)file.GetLength());

            int mode = source switch
            {
                FrameSource.Image => (int)MediaPipeTask.VisionRunningMode.RunningModeImage,
                FrameSource.Video => (int)MediaPipeTask.VisionRunningMode.RunnineModeVideo,
                _ => (int)MediaPipeTask.VisionRunningMode.RunningModeLiveStream,
            };

            task = Create();
            Initialize(task, opts, mode);

            this.source = source;
        }

        int time = (int)Time.GetTicksMsec();

        if (source is FrameSource.Image)
        {
            return Transform(DetectFromImage(task, image, default));
        }
        else if (source is FrameSource.Video)
        {
            return Transform(DetectFromVideo(task, image, default, time));
        }
        else if (source is FrameSource.Stream)
        {
            reset ??= new ManualResetEventSlim();

            reset.Reset();
            Detect(task, image, default, time);
            reset.Wait();

            if (output is not null)
            {
                var transformed = Transform(output);
                output = null;
                return transformed;
            }
            else
            {
                throw new TimeoutException();
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(source));
        }
    }

    public sealed override void Dispose()
    {
        if (task is not null)
        {
            Destroy(task);
            task.Dispose();
            task = null;
        }

        GC.SuppressFinalize(this);
    }

    protected virtual void OnAsyncResult(TTaskOutput result, MediaPipeImage image, int timestamp)
    {
        output = result;
        reset.Set();
    }

    protected abstract TTask Create();
    protected abstract void Destroy(TTask task);
    protected abstract void Initialize(TTask task, MediaPipeTaskBaseOptions options, int mode);
    protected abstract TOutput Transform(TTaskOutput output);
    protected abstract TTaskOutput DetectFromImage(TTask task, MediaPipeImage image, Rect2 region);
    protected abstract TTaskOutput DetectFromVideo(TTask task, MediaPipeImage image, Rect2 region, int timestamp);
    protected abstract void Detect(TTask task, MediaPipeImage image, Rect2 region, int timestamp);
}
