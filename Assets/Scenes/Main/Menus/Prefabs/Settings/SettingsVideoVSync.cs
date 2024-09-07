using UnityEngine;

public class SettingsVideoVSync : MenuItemOptionHandler
{
    protected override int GetDefault()
    {
        return PlayerPrefs.GetInt($"settings.video.vsync", QualitySettings.vSyncCount);
    }

    protected override void SelectionChanged(int index, string value)
    {
        QualitySettings.vSyncCount = index;
    }
}
