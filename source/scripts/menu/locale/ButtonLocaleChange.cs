using Godot;
using Godot.Collections;
using Party.Game.Config;

namespace Party.Game.Menu.Locale;

[Tool]
public partial class ButtonLocaleChange : ButtonConfirmHandler
{
    [Export(PropertyHint.Enum)]
    public string Locale { get; set; } = "en";

    public override void _ValidateProperty(Dictionary property)
    {
        if (property["name"].As<StringName>() == PropertyName.Locale)
        {
            property["hint_string"] = string.Join(',', TranslationServer.GetLoadedLocales());
        }
    }

    protected override void OnConfirm()
    {
        ConfigManager.Current?.SetValue("general", "locale", Locale);
        SceneStack.Current?.Exit();
    }
}
