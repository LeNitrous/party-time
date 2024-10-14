using System;
using System.Diagnostics.CodeAnalysis;
using Godot;
using Party.Game.Experience.Events;

namespace Party.Game.Experience.Directors;

public sealed class GameDirectorStandard : GameDirectorRandomized
{
    public static bool Shift = true;
    public static GameLength Length = GameLength.Medium;

    private int combo;
    private int current;

    public override bool Next([NotNullWhen(true)] out GameEvent game)
    {
        int next = current + 1;

        if (next >= getRoundCount(Length))
        {
            game = null;
            return false;
        }

        current = next;
        return base.Next(out game);
    }

    public override float GetSpeed()
    {
        return Shift ? Mathf.Lerp(1.0f, 1.5f, Mathf.Remap(combo, 0.0f, 5.0f, 0.0f, 1.0f)) : base.GetSpeed();
    }

    public override void OnFinish(Completion completion)
    {
        if (completion is Completion.Win or Completion.WinTimeout)
        {
            combo++;
        }
        else
        {
            combo = 0;
        }
    }

    public enum GameLength
    {
        Short = 1,

        Medium,

        Long,
    }

    private static int getRoundCount(GameLength length)
    {
        return length switch
        {
            GameLength.Short => 10,
            GameLength.Medium => 15,
            _ => 20,
        };
    }
}