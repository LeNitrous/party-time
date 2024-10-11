using Godot;
using Party.Game.Experience.Events;

namespace Party.Game.Experience.Directors;

public sealed partial class GameDirectorDummy : GameDirector
{
    private int round;
    private readonly int rounds;

    public GameDirectorDummy(int rounds = -1)
    {
        this.rounds = rounds;
    }

    public override bool Next(out GameEvent game)
    {
        int next = round++;

        if (next > rounds)
        {
            game = null;
            return false;
        }
        else
        {
            game = GD.Load<PackedScene>("res://scenes/minigames/minigame_dummy.tscn").Instantiate<GameEvent>();
            return true;
        }
    }

    public override float GetSpeed()
    {
        float s = (float)Mathf.Remap(round, 0, rounds, 1.0, 1.5);
        return s;
    }
}
