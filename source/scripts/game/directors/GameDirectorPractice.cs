using System.Diagnostics.CodeAnalysis;
using Godot;
using Party.Game.Experience.Events;

namespace Party.Game.Experience.Directors;

public sealed class GameDirectorPractice : GameDirector
{
    public static PackedScene Event = null;

    public override bool Next([NotNullWhen(true)] out GameEvent game)
    {
        if (Event is not null)
        {
            game = Event.Instantiate<GameEvent>();
            return true;
        }
        else
        {
            game = null;
            return false;
        }
    }
}
