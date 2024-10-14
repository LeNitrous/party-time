using System.Threading;
using GDExtension.Wrappers;
using Godot;
using Party.Game.Camera;
using Party.Game.Detection;

namespace Party.Game.Experience.Events;

public abstract partial class GameEventDetectionTask<TOutput> : GameEvent
{
    private TOutput output;
    private bool hasOutput;
    private DetectionTask<TOutput> task;
    private readonly ManualResetEventSlim reset = new ManualResetEventSlim(false);

    public sealed override void _Ready()
    {
        task = CreateTask();
    }

    public sealed override void _ExitTree()
    {
        reset.Wait();
        reset.Dispose();
        task?.Dispose();
        task = null;
    }

    public sealed override void _Process(double delta)
    {
        if (hasOutput)
        {
            if (!IsCompletionSet && IsDetectionValid(output))
            {
                OnDetect(output);
            }

            hasOutput = false;
        }
    }

    public sealed override void CameraFrameReceived(MediaPipeImage image)
    {
        if (task is not null && !hasOutput)
        {
            reset.Reset();
            output = task.Detect(image, FrameSource.Stream);
            hasOutput = true;
            reset.Set();
        }
    }

    protected abstract void OnDetect(TOutput output);

    protected abstract bool IsDetectionValid(TOutput output);

    protected abstract DetectionTask<TOutput> CreateTask();

    protected abstract DetectionAnnotator<TOutput> CreateAnnotator();
}
