using System.Diagnostics.CodeAnalysis;
using Godot;
using Party.Game.Experience.Events;

namespace Party.Game.Experience.Directors;

public sealed class GameDirectorPractice : GameDirector
{
    private readonly PackedScene scene;

    public GameDirectorPractice(PackedScene scene)
    {
        this.scene = scene;
    }

    public override bool Next([NotNullWhen(true)] out GameEvent game)
    {
        if (scene is not null)
        {
            game = scene.Instantiate<GameEvent>();
            return true;
        }
        else
        {
            game = null;
            return false;
        }
    }
}
