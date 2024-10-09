using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;

namespace Party.Game.Experience.Managers;

public sealed partial class GameStateManager : Node
{
    public static GameStateManager Current { get; private set; }

    public int Score
    {
        get => score;
        private set
        {
            if (score == value)
            {
                return;
            }

            EmitSignal(SignalName.ScoreChanged, score = value);
        }
    }

    public int Combo
    {
        get => combo;
        private set
        {
            if (combo == value)
            {
                return;
            }

            EmitSignal(SignalName.ComboChanged, combo = value);
        }
    }

    public int Round
    {
        get => round;
        private set
        {
            if (round == value)
            {
                return;
            }

            EmitSignal(SignalName.RoundChanged, round = value);
        }
    }

    public int ComboMaximum { get; private set; }

    public TimeSpan Duration => stopwatch?.Elapsed ?? TimeSpan.Zero;

    private int score;
    private int combo;
    private int round;
    private Stopwatch stopwatch;
    private readonly Queue<Completion> completions = new Queue<Completion>();

    public override void _EnterTree()
    {
        if (Current is null)
        {
            Current = this;
        }
    }

    public override void _ExitTree()
    {
        if (Current is not null && Current == this)
        {
            Current = null;
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationPaused)
        {
            stopwatch?.Stop();
        }

        if (what == NotificationUnpaused)
        {
            stopwatch?.Start();
        }
    }

    private void onCompletionChanged(Completion completion)
    {
        if (completion is Completion.Win or Completion.WinTimeout)
        {
            Score++;
            Combo++;
        }
        else
        {
            Combo = 0;
        }

        if (Combo > ComboMaximum)
        {
            ComboMaximum = Combo;
        }

        completions.Enqueue(completion);
    }

    private void onPhaseChanged(Phase phase)
    {
        if (phase is Phase.Starting)
        {
            Round++;
        }

        if (phase is Phase.Starting && stopwatch is null)
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        if (phase is Phase.Epilogue)
        {
            stopwatch?.Stop();
        }
    }

    [Signal]
    public delegate void ScoreChangedEventHandler(int score);

    [Signal]
    public delegate void ComboChangedEventHandler(int combo);

    [Signal]
    public delegate void RoundChangedEventHandler(int round);
}
