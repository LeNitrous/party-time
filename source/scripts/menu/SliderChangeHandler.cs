using Godot;

namespace Party.Game.Menu;

public abstract partial class SliderChangeHandler : Component<Slider>
{
    protected abstract void OnCurrentChanged(float value);

    protected sealed override void OnAttach()
    {
        if (!Engine.IsEditorHint())
        {
            Parent.Connect(Slider.SignalName.CurrentChanged, Callable.From<float>(OnCurrentChanged));
        }
    }

    protected sealed override void OnDetach()
    {
        if (!Engine.IsEditorHint())
        {
            Parent.Disconnect(Slider.SignalName.CurrentChanged, Callable.From<float>(OnCurrentChanged));
        }
    }
}
