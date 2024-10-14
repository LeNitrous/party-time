namespace Party.Game.Experience.Events;

public sealed partial class GameEventStandStill : GameEventMovementCheck
{
    public override double Duration => 8.0;

    public override Completion GetCompletionOnTimeout()
    {
        return Completion.WinTimeout;
    }

    protected override void OnMovement()
    {
        Trigger(Completion.Lose); 
    }
}
