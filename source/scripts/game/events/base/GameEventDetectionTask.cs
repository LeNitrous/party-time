using Godot;
using Party.Game.Camera;
using Party.Game.Detection;

namespace Party.Game.Experience.Events;

public abstract partial class GameEventDetectionTask<TOutput> : GameEvent
{
    private TOutput output;
    private bool hasOutput;
    private DetectionTask<TOutput> task;

    public sealed override void _Ready()
    {
        if (CameraService.Current is not null)
        {
            CameraService.Current.OnFrame += onCameraFrame;
        }

        task = CreateTask();
    }

    public sealed override void _ExitTree()
    {
        if (CameraService.Current is not null)
        {
            CameraService.Current.OnFrame -= onCameraFrame;
        }

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

    private void onCameraFrame(Image image)
    {
        if (task is not null && !hasOutput)
        {
            output = task.Detect(image, FrameSource.Stream, CameraService.Current.Accelerated);
            hasOutput = true;
        }
    }

    protected abstract void OnDetect(TOutput output);

    protected abstract bool IsDetectionValid(TOutput output);

    protected abstract DetectionTask<TOutput> CreateTask();

    protected abstract DetectionAnnotator<TOutput> CreateAnnotator();
}
