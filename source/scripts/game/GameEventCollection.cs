using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Party.Game.Experience;

[GlobalClass]
public partial class GameEventCollection : Resource, IReadOnlyList<GameEventMetadata>
{
    public int Count => events.Count;

    public GameEventMetadata this[int index] => events[index];

    [Export]
    private Array<GameEventMetadata> events;

    public IEnumerator<GameEventMetadata> GetEnumerator()
    {
        return events.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
