using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Screen))]
public class ScreenBackground : MonoBehaviour
{
    [SerializeField]
    private Color accent;

    [SerializeField]
    private float accentFadeTime = 0.5f;

    [SerializeField]
    private bool pattern;

    private Screen screen;
    private ScreenBackgroundController background;

    private void OnScreenEnteringOrResuming()
    {
        if (screen == null)
        {
            return;
        }

        if (background == null)
        {
            background = FindFirstObjectByType<ScreenBackgroundController>();
        }

        if (background != null)
        {
            background.ShowPattern = pattern;
            background.ShowEffects = screen is MenuScreen;
            
            if (accentFadeTime == 0.0f)
            {
                background.Color = accent;
            }
            else
            {
                StartCoroutine(Fade(accent, accentFadeTime));
            }
        }
    }

    private void OnScreenEnteringOrResuming(Screen screen, GameObject prev, GameObject next)
    {
        OnScreenEnteringOrResuming();
    }

    private IEnumerator Fade(Color end, float duration)
    {
        return Fade(background.Color, end, duration);
    }

    private IEnumerator Fade(Color start, Color end, float duration)
    {
        float current = 0.0f;
        
        if (background.Color != start)
        {
            background.Color = start;
        }

        while (current < duration)
        {
            current += Time.deltaTime;
            background.Color = Color.Lerp(start, end, current / duration);
            yield return null;
        }

        yield break;
    }

    private void OnEnable()
    {
        if (screen == null)
        {
            screen = GetComponent<Screen>();
        }

        if (screen != null)
        {
            screen.Entered += OnScreenEnteringOrResuming;
            screen.Resumed += OnScreenEnteringOrResuming;
        }
    }

    private void Start()
    {
        OnScreenEnteringOrResuming();
    }

    private void OnDisable()
    {
        if (screen != null)
        {
            screen.Entered -= OnScreenEnteringOrResuming;
            screen.Resumed -= OnScreenEnteringOrResuming;
        }
    }
}
