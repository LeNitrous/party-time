using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettingsVideoRefreshRate : MenuItemOptionHandler
{
    private List<RefreshRate> rates;
    private string[] options;
    private int previousW;
    private int previousH;

    protected override string[] GetOptions()
    {
        var w = UnityEngine.Screen.currentResolution.width;
        var h = UnityEngine.Screen.currentResolution.height;

        if (w == previousW && h == previousH)
        {
            return options;
        }

        rates ??= new List<RefreshRate>();
        rates.Clear();

        var curr = UnityEngine.Screen.currentResolution;

        for (int i = 0; i < UnityEngine.Screen.resolutions.Length; i++)
        {
            var resolution = UnityEngine.Screen.resolutions[i];

            if (resolution.width != curr.width && resolution.height != curr.height)
            {
                continue;
            }

            rates.Add(resolution.refreshRateRatio);
        }

        previousW = w;
        previousH = h;

        return options = rates.Select(r => r.ToString() + "Hz").ToArray();
    }

    protected override int GetDefault()
    {
        if (rates == null)
        {
            return 0;
        }

        uint numerator = (uint)PlayerPrefs.GetInt("settings.video.refresh.numerator", (int)UnityEngine.Screen.currentResolution.refreshRateRatio.numerator);
        uint denominator = (uint)PlayerPrefs.GetInt("settings.video.refresh.numerator", (int)UnityEngine.Screen.currentResolution.refreshRateRatio.denominator);

        return Mathf.Min(0, rates.IndexOf(new RefreshRate { numerator = numerator, denominator = denominator }));
    }

    protected override void SelectionChanged(int index, string value)
    {
        var curr = UnityEngine.Screen.currentResolution;
        PlayerPrefs.SetInt("settings.video.refresh.numerator", (int)rates[index].numerator);
        PlayerPrefs.SetInt("settings.video.refresh.denominator", (int)rates[index].denominator);
        UnityEngine.Screen.SetResolution(curr.width, curr.height, UnityEngine.Screen.fullScreenMode, rates[index]);
    }
}
