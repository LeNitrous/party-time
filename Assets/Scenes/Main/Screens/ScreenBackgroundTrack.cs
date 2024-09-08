using UnityEngine;

[RequireComponent(typeof(Screen))]
public class ScreenBackgroundTrack : MonoBehaviour
{
    [SerializeField]
    private SoundTrackMetadata track;

    private Screen screen;

    private void OnScreenEntered()
    {
        if (SoundTrackController.Current != null && track != null)
        {
            SoundTrackController.Current.Play(track);
        }
    }

    private void OnScreenResumed()
    {
        if (SoundTrackController.Current != null && track != null)
        {
            SoundTrackController.Current.Play(track, true);
        }
    }

    private void OnScreenEntered(Screen screen, GameObject prev, GameObject next)
    {
        OnScreenEntered();
    }

    private void OnScreenResumed(Screen screen, GameObject prev, GameObject next)
    {
        OnScreenResumed();
    }

    private void OnEnable()
    {
        if (screen == null)
        {
            screen = GetComponent<Screen>();
        }

        if (screen != null)
        {
            screen.Entered += OnScreenEntered;
            screen.Resumed += OnScreenResumed;
        }
    }

    private void Start()
    {
        OnScreenEntered();
    }

    private void OnDisable()
    {
        if (screen != null)
        {
            screen.Entered -= OnScreenEntered;
            screen.Resumed -= OnScreenResumed;
        }
    }
}
