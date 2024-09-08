using UnityEngine;

public class SettingsVideoQuality : MenuItemOptionHandler
{
    protected override int GetDefault()
    {
        return QualitySettings.GetQualityLevel();
    }

    protected override string[] GetOptions()
    {
        return QualitySettings.names;
    }

    protected override void SelectionChanged(int index, string value)
    {
        QualitySettings.SetQualityLevel(index);
    }
}
