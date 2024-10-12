using System.Diagnostics.CodeAnalysis;
using Godot;
using Party.Game.Experience.Events;

namespace Party.Game.Experience.Directors;

public class GameDirectorRandomized : GameDirector
{
    private GameEventCollection events;

    public override bool Next([NotNullWhen(true)] out GameEvent game)
    {
        if (events is null)
        {
            events = (GameEventCollection)GD.Load<Resource>(ProjectSettings.GetSetting("application/game/collection", string.Empty).AsString());
        }

        game = events.GetRandomWeighted(t => t.Weight).Scene.Instantiate<GameEvent>();
        return true;
    }
}
