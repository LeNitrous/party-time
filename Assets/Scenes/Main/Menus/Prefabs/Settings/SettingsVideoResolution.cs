using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettingsVideoResolution : MenuItemOptionHandler
{
    private readonly struct ScreenResolution : IEquatable<ScreenResolution>
    {
        public readonly int Index;
        public readonly int Width;
        public readonly int Height;

        public ScreenResolution(int index, int width, int height)
        {
            Index = index;
            Width = width;
            Height = height;
        }

        public bool Equals(ScreenResolution other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            return obj is ScreenResolution other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height);
        }

        public override string ToString()
        {
            return $"{Width} x {Height}";
        }
    }

    private List<ScreenResolution> resolutions;

    protected override string[] GetOptions()
    {
        PopulateResolutions();
        return resolutions.Select(x => x.ToString()).ToArray();
    }

    protected override int GetDefault()
    {
        PopulateResolutions();
        int w = PlayerPrefs.GetInt("settings.video.width", UnityEngine.Screen.currentResolution.width);
        int h = PlayerPrefs.GetInt("settings.video.height", UnityEngine.Screen.currentResolution.height);
        var resolution = new ScreenResolution(0, w, h);
        return resolutions.IndexOf(resolution);
    }

    protected override void SelectionChanged(int index, string value)
    {
        PopulateResolutions();
        var curr = UnityEngine.Screen.currentResolution;
        var next = resolutions[index];
        PlayerPrefs.SetInt("settings.video.width", next.Width);
        PlayerPrefs.SetInt("settings.video.height", next.Height);
        UnityEngine.Screen.SetResolution(next.Width, next.Height, UnityEngine.Screen.fullScreenMode, curr.refreshRateRatio);
    }

    private void PopulateResolutions()
    {
        if (resolutions != null)
        {
            return;
        }

        resolutions = new List<ScreenResolution>();

        for (int i = 0; i < UnityEngine.Screen.resolutions.Length; i++)
        {
            var resolution = new ScreenResolution(i, UnityEngine.Screen.resolutions[i].width, UnityEngine.Screen.resolutions[i].height);

            if (resolutions.Contains(resolution))
            {
                continue;
            }

            resolutions.Add(resolution);
        }
    }
}
