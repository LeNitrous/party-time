using Godot;

namespace Party.Game.Experience;

public sealed partial class HUDLabelValueSetter : Component<HUDLabel>
{
    private void onValueChanged(Variant value)
    {
        Parent.Value = value.AsString();
    }
}
