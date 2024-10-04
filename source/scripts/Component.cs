using Godot;

namespace Party.Game;

public abstract partial class Component<TParent> : Node
    where TParent : Node
{
    protected TParent Parent { get; private set; }

    public override void _Notification(int what)
    {
        if (what == NotificationParented)
        {
            Parent = GetParent<TParent>();
            OnAttach();
        }

        if (what == NotificationUnparented)
        {
            OnDetach();
            Parent = null;
        }
    }

    protected virtual void OnAttach()
    {
    }

    protected virtual void OnDetach()
    {
    }
}
