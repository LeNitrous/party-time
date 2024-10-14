namespace Party.Game.Experience.Events;

public sealed partial class GameEventStandStill : GameEventMovementCheck
{
    public override double Duration => 8.0;

    private bool isGracePeriod = true;

    public override void OnStart()
    {
        GetTree().CreateTimer(1.0).Timeout += () => isGracePeriod = false;
    }

    public override Completion GetCompletionOnTimeout()
    {
        return Completion.WinTimeout;
    }

    protected override void OnMovement()
    {
        Trigger(Completion.Lose); 
    }

    protected override bool ShouldDetect()
    {
        return !isGracePeriod;
    }
}
