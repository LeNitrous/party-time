using UnityEngine;
using UnityEngine.Audio;

public class SettingsAudioMenuItem : MonoBehaviour
{
    [SerializeField]
    private AudioMixerGroup group;

    private MenuItemOptions options;

    private void OnSelectionChanged(MenuItemOptions sender)
    {
        if (group == null)
        {
            return;
        }

        string name = group.name.ToLowerInvariant();
        float value = Mathf.Lerp(-80.0f, 0.0f, Mathf.Clamp(sender.Current / (float)(sender.Options.Length - 1), 0.0f, 1.0f));
        group.audioMixer.SetFloat(name, value);
        PlayerPrefs.SetString($"settings.audio.{name}", name);
    }

    private void OnEnable()
    {
        if (options == null)
        {
            options = GetComponent<MenuItemOptions>();
        }

        options.OnSelectionChanged += OnSelectionChanged;
        
        if (group.audioMixer.GetFloat(group.name.ToLowerInvariant(), out float value))
        {
            options.Current = (int)Mathf.Lerp(0, options.Options.Length - 1, Mathf.InverseLerp(-80.0f, 0.0f, value));
        }
    }

    private void OnDisable()
    {
        options.OnSelectionChanged -= OnSelectionChanged;
    }
}
