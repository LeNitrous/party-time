using Godot;
using Godot.Collections;
using Party.Game.Config;

namespace Party.Game.Menu.Settings;

[Tool]
public sealed partial class SliderAudioVolume : SliderChangeHandler
{
    [Export(PropertyHint.Enum)]
    public int Bus { get; set; }

    public override void _Ready()
    {
        Parent.Current = ConfigManager.Current?.GetValue("sound", AudioServer.GetBusName(Bus).ToLowerInvariant(), Parent.Maximum) ?? Parent.Maximum;
    }

    public override void _ValidateProperty(Dictionary property)
    {
        if (property["name"].As<StringName>() == PropertyName.Bus)
        {
            string output = "";

            for (int i = 0; i < AudioServer.BusCount; i++)
            {
                if (i > 0)
                {
                    output += ',';
                }

                output += AudioServer.GetBusName(i);
            }

            property["hint_string"] = output;
        }
    }

    protected override void OnCurrentChanged(float value)
    {
        ConfigManager.Current?.SetValue("sound", AudioServer.GetBusName(Bus).ToLowerInvariant(), value);
    }
}
