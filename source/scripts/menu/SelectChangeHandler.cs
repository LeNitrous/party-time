using Godot;

namespace Party.Game.Menu;

public abstract partial class SelectChangeHandler : Component<Select>
{
    protected abstract void OnCurrentChanged(int value);

    protected sealed override void OnAttach()
    {
        Parent.Connect(Select.SignalName.SelectionChanged, Callable.From<int>(OnCurrentChanged));
    }

    protected sealed override void OnDetach()
    {
        Parent.Disconnect(Select.SignalName.SelectionChanged, Callable.From<int>(OnCurrentChanged));
    }
}
