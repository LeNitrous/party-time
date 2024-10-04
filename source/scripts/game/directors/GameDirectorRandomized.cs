using System.Diagnostics.CodeAnalysis;
using Party.Game.Experience.Events;

namespace Party.Game.Experience.Directors;

public class GameDirectorRandomized : GameDirector
{
    public override bool Next([NotNullWhen(true)] out GameEvent game)
    {
        throw new System.NotImplementedException();
    }
}
