using System;
using System.Threading;
using System.Threading.Tasks;
using GDExtension.Wrappers;
using Godot;

namespace Party.Game.Detection;

public abstract class DetectionTask : IDisposable
{
    public abstract void Dispose();
}

public abstract class DetectionTask<TOutput> : DetectionTask
{
    public abstract TOutput Detect(Image image, FrameSource sourceKind, bool isHardwareAccelerated);
}

public abstract class DetectionTask<TTask, TTaskOutput, TOutput> : DetectionTask<TOutput>
    where TTask : MediaPipeTask
    where TTaskOutput : RefCounted
{
    private TTask task;
    private TTaskOutput output;
    private bool isHardwareAccelerated;
    private FrameSource source;
    private AutoResetEvent reset;
    private MediaPipeImage frame;
    private readonly string taskFilePath;

    protected DetectionTask(string taskFilePath)
    {
        this.taskFilePath = taskFilePath;
    }

    public sealed override TOutput Detect(Image image, FrameSource source, bool isHardwareAccelerated)
    {
        if (task is null || this.source != source || this.isHardwareAccelerated != isHardwareAccelerated)
        {
            if (task is not null)
            {
                Destroy(task);
                task.Dispose();
            }

            using var file = FileAccess.Open(taskFilePath, FileAccess.ModeFlags.Read);
            using var opts = MediaPipeTaskBaseOptions.Instantiate();
            opts.Delegate = (int)(isHardwareAccelerated ? MediaPipeTaskBaseOptions.DelegateEnum.DelegateGpu : MediaPipeTaskBaseOptions.DelegateEnum.DelegateCpu);
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
            this.isHardwareAccelerated = isHardwareAccelerated;
        }

        frame ??= MediaPipeImage.Instantiate();
        frame.SetImage(image);

        int time = (int)Time.GetTicksMsec();
        var area = new Rect2I(Vector2I.Zero, image.GetSize());

        if (source is FrameSource.Image)
        {
            return Transform(DetectFromImage(task, frame, area));
        }
        else if (source is FrameSource.Video)
        {
            return Transform(DetectFromVideo(task, frame, area, time));
        }
        else if (source is FrameSource.Stream)
        {
            reset ??= new AutoResetEvent(false);

            Detect(task, frame, area, time);
            reset.WaitOne();

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
