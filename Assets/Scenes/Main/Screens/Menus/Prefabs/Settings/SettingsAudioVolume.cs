using UnityEngine;
using UnityEngine.Audio;

public class SettingsAudioVolume : MenuItemOptionHandler
{
    [SerializeField]
    private AudioMixerGroup group;

    protected override int GetDefault()
    {
        string name = group.name.ToLowerInvariant();
        string keys = $"settings.audio.{name}";
        float value;

        if (PlayerPrefs.HasKey(keys))
        {
            value = PlayerPrefs.GetFloat(keys);
        }
        else
        {
            group.audioMixer.GetFloat(group.name.ToLowerInvariant(), out value);
        }

        return (int)Mathf.Lerp(0, 10, Mathf.InverseLerp(-80.0f, 0.0f, value));
    }

    protected override void SelectionChanged(int index, string option)
    {
        if (group == null)
        {
            return;
        }

        string name = group.name.ToLowerInvariant();
        float value = Mathf.Lerp(-80.0f, 0.0f, Mathf.Clamp(index / 10.0f, 0.0f, 1.0f));

        if (!group.audioMixer.SetFloat(name, value))
        {
            Debug.Log("Failed to set audio volume.");
        }

        PlayerPrefs.SetFloat($"settings.audio.{name}", value);
    }
}
