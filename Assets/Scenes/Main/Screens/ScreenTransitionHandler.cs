using System;
using UnityEngine;
using UnityEngine.Events;

public class ScreenTransitionHandler : MonoBehaviour
{
    [SerializeField, Tooltip("Called when the screen is entered.")]
    private ScreenTransitionEvent entered;

    [SerializeField, Tooltip("Called when the screen is resumed.")]
    private ScreenTransitionEvent resumed;

    [SerializeField, Tooltip("Called when the screen is suspend.")]
    private ScreenTransitionEvent suspend;

    [SerializeField, Tooltip("Called when the screen is exiting.")]
    private ScreenTransitionEvent exiting;

    private Screen screen;

    private void OnEntered(Screen screen, GameObject prev, GameObject next)
    {
        entered?.Invoke(new ScreenTransitionEventArgs(prev, next));
    }

    private void OnResumed(Screen screen, GameObject prev, GameObject next)
    {
        resumed?.Invoke(new ScreenTransitionEventArgs(prev, next));
    }

    private void OnSuspend(Screen screen, GameObject prev, GameObject next)
    {
        suspend?.Invoke(new ScreenTransitionEventArgs(prev, next));
    }

    private void OnExiting(Screen screen, GameObject prev, GameObject next)
    {
        exiting?.Invoke(new ScreenTransitionEventArgs(prev, next));
    }

    private void OnEnable()
    {
        if (screen == null)
        {
            screen = GetComponent<Screen>();
        }

        if (screen != null)
        {
            screen.Entered += OnEntered;
            screen.Resumed += OnResumed;
            screen.Suspend += OnSuspend;
            screen.Exiting += OnExiting;
        }
    }

    private void OnDisable()
    {
        if (screen != null)
        {
            screen.Entered -= OnEntered;
            screen.Resumed -= OnResumed;
            screen.Suspend -= OnSuspend;
            screen.Exiting -= OnExiting;
        }
    }
}

public struct ScreenTransitionEventArgs
{
    public GameObject Previous;
    public GameObject Next;

    public ScreenTransitionEventArgs(GameObject prev, GameObject next)
    {
        Previous = prev;
        Next = next;
    }
}

[Serializable]
public class ScreenTransitionEvent : UnityEvent<ScreenTransitionEventArgs>
{
}
