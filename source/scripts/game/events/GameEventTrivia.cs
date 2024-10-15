using Godot;

namespace Party.Game.Experience.Events;

public abstract partial class GameEventTrivia : GameEventHeadTracker
{
    private Vector2 position;

    public sealed override Completion GetCompletionOnTimeout()
    {
        if (GetNode<Control>("%L").GetRect().HasPoint(position) && IsCorrect(0))
        {
            return Completion.WinTimeout;
        }

        if (GetNode<Control>("%R").GetRect().HasPoint(position) && IsCorrect(1))
        {
            return Completion.WinTimeout;
        }

        return Completion.LoseTimeout;
    }

    protected sealed override void OnDetect(Vector2 center, float radius)
    {
        position = center;
    }

    protected abstract bool IsCorrect(int index);
}
