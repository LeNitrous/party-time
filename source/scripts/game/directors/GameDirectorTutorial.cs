using System.Diagnostics.CodeAnalysis;
using Godot;
using Party.Game.Experience.Events;

namespace Party.Game.Experience.Directors;

public sealed class GameDirectorTutorial : GameDirector
{
    private int progress = -1;
    private readonly GameEventCollection collection;

    public GameDirectorTutorial()
    {
        collection = (GameEventCollection)GD.Load<Resource>("res://minigames/minigame_collection_tutorial.tres");
    }

    public override bool Next([NotNullWhen(true)] out GameEvent game)
    {
        int next = progress + 1;

        if (next >= collection.Count)
        {
            game = null;
            return false;
        }
        else
        {
            game = collection[progress = next].Scene.Instantiate<GameEvent>();
            return true;
        }
    }
}
