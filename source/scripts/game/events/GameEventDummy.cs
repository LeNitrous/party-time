namespace Party.Game.Experience.Events;

public sealed partial class GameEventDummy : GameEvent
{
    public override Completion Timeout => Completion.WinTimeout;
}
