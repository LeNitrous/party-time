namespace Party.Game.Experience.Events;

public sealed partial class GameEventDummy : GameEvent
{
    public override double Duration => 15.0;
    public override Completion Timeout => Completion.LoseTimeout;
}
