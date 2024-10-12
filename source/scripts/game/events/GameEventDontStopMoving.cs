namespace Party.Game.Experience.Events;

public sealed partial class GameEventDontStopMoving : GameEventMovementCheck
{
    public override double Duration => 8.0;

    private int framesMoved;

    public override Completion GetCompletionOnTimeout()
    {
        return framesMoved >= requiredMovementFrames ? Completion.WinTimeout : Completion.LoseTimeout;
    }

    protected override void OnMovement()
    {
        framesMoved++;
    }

    private const int requiredMovementFrames = 30;
}
