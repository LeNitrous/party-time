using System.Collections.Generic;
using Godot;

namespace Party.Game.Experience.Managers;

public sealed partial class GameStateManager : Node
{
    public int Score
    {
        get => score;
        set
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
        set
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
        set
        {
            if (round == value)
            {
                return;
            }

            EmitSignal(SignalName.RoundChanged, round = value);
        }
    }

    private int score;
    private int combo;
    private int round;
    private readonly Queue<Completion> completions = new Queue<Completion>();

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

        completions.Enqueue(completion);
    }

    private void onPhaseChanged(Phase phase)
    {
        if (phase is Phase.Starting)
        {
            Round++;
        }
    }

    [Signal]
    public delegate void ScoreChangedEventHandler(int score);

    [Signal]
    public delegate void ComboChangedEventHandler(int combo);

    [Signal]
    public delegate void RoundChangedEventHandler(int round);
}
