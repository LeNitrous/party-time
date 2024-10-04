using System.Diagnostics.CodeAnalysis;
using Party.Game.Experience.Events;

namespace Party.Game.Experience.Directors;

public abstract class GameDirector
{
    public abstract bool Next([NotNullWhen(true)] out GameEvent game);

    public virtual void OnStart()
    {
    }

    public virtual void OnFinish(Completion completion)
    {
    }

    public virtual float GetSpeed()
    {
        return 1.0f;
    }
}
