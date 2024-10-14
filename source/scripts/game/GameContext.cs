using GDExtension.Wrappers;
using Godot;
using Party.Game.Camera;
using Party.Game.Experience.Directors;
using Party.Game.Experience.Events;

namespace Party.Game.Experience;

public sealed partial class GameContext : Node
{
    public static GameDirector Director = null;

    private GameEvent game;
    private Phase current;
    private CanvasLayer view;
    private Timer time;
    private Timer wait;
    private GameDirector director;
    private Completion completion;
    private TextureRect camera;

    public override void _Ready()
    {
        if (CameraService.Current is not null)
        {
            CameraService.Current.OnFrame += onCameraFrame;
            CameraService.Current.Start();
        }

        time = GetNode<Timer>("Time");
        wait = GetNode<Timer>("Wait");
        view = GetNode<CanvasLayer>("%View");
        camera = GetNode<TextureRect>("%Camera");
    }

    public override void _ExitTree()
    {
        if (CameraService.Current is not null)
        {
            CameraService.Current.OnFrame -= onCameraFrame;
            CameraService.Current.Close();
        }
    }

    public override void _Process(double delta)
    {
        void raise(Completion state)
        {
            if (state is not Completion.None && time.TimeLeft > 5.0)
            {
                time.CallThreadSafe(Timer.MethodName.Start, 5.0);
            }

            completion = state;
            game.CompletionChanged -= raise;

            GD.Print(nameof(GameContext), " :: raised \"", state, "\" completion state");
            CallThreadSafe(GodotObject.MethodName.EmitSignal, SignalName.CompletionRaised, (int)state);
        }

        if (director == null)
        {
            return;
        }

        if (current is Phase.InProgress)
        {
            if (time.TimeLeft > 0.0)
            {
                return;
            }
        }
        else
        {
            if (wait.TimeLeft > 0.0)
            {
                return;
            }
        }

        var prev = current;
        var next = getNextPhase(current);

        if (next is Phase.Prologue or Phase.Transition)
        {
            Engine.TimeScale = director.GetSpeed();

            if (director.Next(out game))
            {
                game.CompletionChanged += raise;
                game.ProcessMode = ProcessModeEnum.Disabled;
                view.Hide();
                view.AddChild(game);
            }
            else
            {
                next = Phase.Epilogue;
            }
        }

        if (next is Phase.Starting)
        {
            director.OnStart();

            if (game.Duration > 0.0)
            {
                time.Paused = false;
                time.Start(game.Duration);
            }

            view.Show();
            game.ProcessMode = ProcessModeEnum.Inherit;
        }

        if (next is not Phase.InProgress)
        {
            double duration = next switch
            {
                Phase.Starting => Duration.Starting,
                Phase.Ending => Duration.Ending,
                Phase.Prologue => Duration.Prologue,
                Phase.Epilogue => Duration.Epilogue,
                Phase.Transition => Duration.Transition,
                _ => default,
            };

            if (duration > 0)
            {
                wait.Start(duration);
            }
        }

        if (next is Phase.Ending)
        {
            if (completion == Completion.None)
            {
                raise(game.GetCompletionOnTimeout());
            }

            director.OnFinish(completion);

            game.QueueFree();
            game = null;

            time.Stop();
            time.Paused = true;

            completion = Completion.None;
        }

        if (next is Phase.Epilogue)
        {
            Engine.TimeScale = 1.0;
        }

        if (next is Phase.Finished)
        {
            director = null;
        }

        if (next != prev && next != Phase.Invalid)
        {
            GD.Print(nameof(GameContext), " :: phase changed from \"", prev, "\" to \"", next, "\"");
            EmitSignal(SignalName.PhaseChanged, (int)(current = next));
        }
        else
        {
            GD.PushError(nameof(GameContext), " :: attempted to transition from \"", prev, "\" to \"", next, "\" which is an invalid transition!");
        }
    }

    public void Start()
    {
        Start(Director ?? new GameDirectorDummy(2));
    }

    public void Start(GameDirector director)
    {
        this.director = director;
    }

    private void onCameraFrame(MediaPipeImage mp)
    {
        if (mp.IsGpuImage())
        {
            mp.ConvertToCpu();
        }

        game?.CameraFrameReceived(mp);

        // This forsaken line is causing memory leaks that only happens in C#
        using var image = mp.GetImage();

        if (camera.Texture.GetSize() == image.GetSize())
        {
            camera.Texture.CallDeferred(ImageTexture.MethodName.Update, image);
        }
        else
        {
            camera.Texture.CallDeferred(ImageTexture.MethodName.SetImage, image);
        }

        // DO NOT DO THIS AT ANY COST BUT WE ARE FORCED
        System.GC.Collect(System.GC.MaxGeneration, System.GCCollectionMode.Forced);
        System.GC.WaitForPendingFinalizers();
    }

    private static Phase getNextPhase(Phase phase)
    {
        switch (phase)
        {
            case Phase.None:
                return Phase.Prologue;

            case Phase.Prologue:
                return Phase.Starting;

            case Phase.Starting:
                return Phase.InProgress;

            case Phase.InProgress:
                return Phase.Ending;

            case Phase.Ending:
                return Phase.Transition;

            case Phase.Transition:
                return Phase.Starting;

            case Phase.Epilogue:
                return Phase.Finished;

            default:
                return Phase.Invalid;
        }
    }

    [Signal]
    public delegate void CompletionRaisedEventHandler(Completion completion);

    [Signal]
    public delegate void PhaseChangedEventHandler(Phase phase);

    [Signal]
    public delegate void SpeedChangedEventHandler(float speed);

    public static class Duration
    {
        public const double Starting = 2.46;
        public const double Ending = 2.46;
        public const double Prologue = 13.32;
        public const double Epilogue = 24.24;
        public const double Transition = 1.31;
    }
}
