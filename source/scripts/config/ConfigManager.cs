using Godot;
using System;
using System.Collections.Generic;

namespace Party.Game.Config;

public sealed partial class ConfigManager : Node
{
    public static ConfigManager Current { get; private set; }

    private ConfigFile source;
    private readonly Dictionary<string, Configuration> configurations = new Dictionary<string, Configuration>();

    public override void _Ready()
    {
        if (Current is null)
        {
            Current = this;

            source = new ConfigFile();
            source.Load(path);
            initialize();
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public override void _ExitTree()
    {
        if (Current is not null && Current == this)
        {
            Current = null;
            source.Save(path);
        }
    }

    public void SetValue<[MustBeVariant] T>(string section, string key, T value)
    {
        if (!configurations.TryGetValue($"{section}:{key}", out var config))
        {
            GD.PushError(nameof(ConfigManager), " :: configuration item \"", section, ":", key, "\" does not exist!");
            return;
        }

        var v = Variant.From(value);
        config.Setter.Invoke(v);
        source.SetValue(section, key, v);
    }

    public T GetValue<[MustBeVariant] T>(string section, string key, T value = default)
    {
        return source.GetValue(section, key, Variant.From(value)).As<T>();
    }

    private void initialize()
    {
        add("general", "locale", "en", TranslationServer.SetLocale);
        add("display", "window/size/mode", DisplayServer.WindowMode.Fullscreen, v => DisplayServer.WindowSetMode(v));
        add("display", "window/vsync/vsync_mode", DisplayServer.VSyncMode.Enabled, v => DisplayServer.WindowSetVsyncMode(v));
        add("sound", "master", 100f, v => AudioServer.SetBusVolumeDb(0, Mathf.Remap(v, 0, 100, -80, 0)));
        add("sound", "music", 100f, v => AudioServer.SetBusVolumeDb(1, Mathf.Remap(v, 0, 100, -80, 0)));
        add("sound", "effect", 100f, v => AudioServer.SetBusVolumeDb(2, Mathf.Remap(v, 0, 100, -80, 0)));
        add("sound", "voice", 100f, v => AudioServer.SetBusVolumeDb(3, Mathf.Remap(v, 0, 100, -80, 0)));

        foreach (var config in configurations.Values)
        {
            config.Setter(source.GetValue(config.Section, config.Key, config.Initial));
        }
    }

    private void add<[MustBeVariant] T>(string section, string key, T initial, Action<T> setter)
    {
        configurations.Add($"{section}:{key}", new Configuration(section, key, Variant.From(initial), v => setter(v.As<T>())));
    }

    private const string path = "user://settings.cfg";

    private record Configuration(string Section, string Key, Variant Initial, Action<Variant> Setter);
}
