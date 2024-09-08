using UnityEngine;

public class SettingsVideoPresentation : MenuItemOptionHandler
{
    protected override int GetDefault()
    {
        return PlayerPrefs.GetInt($"settings.video.fullscreen", UnityEngine.Screen.fullScreen ? 1 : 0);
    }

    protected override void SelectionChanged(int index, string value)
    {
        UnityEngine.Screen.fullScreen = index == 1;
        PlayerPrefs.SetInt($"settings.video.fullscreen", index);
    }
}
