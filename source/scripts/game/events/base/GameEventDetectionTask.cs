using System;
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

    public override void _Ready()
    {
        task = CreateTask();
        Init();
    }

    public override void _ExitTree()
    {
        if (reset.Wait(TimeSpan.FromSeconds(5)))
        {
            Exit();
            reset.Dispose();
            task?.Dispose();
            task = null;
        }
        else
        {
            GD.PrintErr(nameof(GameEventDetectionTask<TOutput>), " :: failed to cleanup resources!");
        }
    }

    public override void _Process(double delta)
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

    public sealed override void OnFrame(MediaPipeImage image)
    {
        reset.Reset();

        if (task is not null && !hasOutput)
        {
            output = task.Detect(image, FrameSource.Stream);
            hasOutput = true;
        }

        reset.Set();
    }

    protected virtual void Init()
    {
    }

    protected virtual void Exit()
    {
    }

    protected abstract void OnDetect(TOutput output);

    protected abstract bool IsDetectionValid(TOutput output);

    protected abstract DetectionTask<TOutput> CreateTask();

    protected abstract DetectionAnnotator<TOutput> CreateAnnotator();
}
