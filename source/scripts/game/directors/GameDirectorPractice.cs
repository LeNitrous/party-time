using System.Diagnostics.CodeAnalysis;
using Godot;
using Party.Game.Experience.Events;

namespace Party.Game.Experience.Directors;

public sealed class GameDirectorPractice : GameDirector
{
    public static string Event = string.Empty;

    public override bool Next([NotNullWhen(true)] out GameEvent game)
    {
        game = GD.Load<PackedScene>(Event).Instantiate<GameEvent>();
        return true;
    }
}
