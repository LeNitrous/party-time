using System.Diagnostics.CodeAnalysis;
using Party.Game.Experience.Events;

namespace Party.Game.Experience.Directors;

public sealed class GameDirectorPractice : GameDirector
{
    public override bool Next([NotNullWhen(true)] out GameEvent game)
    {
        throw new System.NotImplementedException();
    }
}
