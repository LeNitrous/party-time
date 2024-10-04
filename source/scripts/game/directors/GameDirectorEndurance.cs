using System.Diagnostics.CodeAnalysis;
using Party.Game.Experience.Events;

namespace Party.Game.Experience.Directors;

public sealed class GameDirectorEndurance : GameDirectorRandomized
{
    private bool hasFailed;

    public override bool Next([NotNullWhen(true)] out GameEvent game)
    {
        if (hasFailed)
        {
            game = null;
            return false;
        }

        return base.Next(out game);
    }

    public override void OnFinish(Completion completion)
    {
        hasFailed = completion is Completion.Lose or Completion.LoseTimeout;
    }
}
