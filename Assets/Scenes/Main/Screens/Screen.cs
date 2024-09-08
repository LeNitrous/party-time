using System;
using System.Collections;
using UnityEngine;

public class Screen : MonoBehaviour
{
    public event Action<Screen, GameObject, GameObject> Entered;

    public event Action<Screen, GameObject, GameObject> Resumed;

    public event Action<Screen, GameObject, GameObject> Suspend;

    public event Action<Screen, GameObject, GameObject> Exiting;

    /// <summary>
    /// Called when the screen has been entered.
    /// </summary>
    public virtual IEnumerator OnEntered(GameObject prev, GameObject next)
    {
        Entered?.Invoke(this, prev, next);
        yield break;
    }

    /// <summary>
    /// Called when the screen has been resumed.
    /// </summary>
    public virtual IEnumerator OnResumed(GameObject prev, GameObject next)
    {
        Resumed?.Invoke(this, prev, next);
        yield break;
    }

    /// <summary>
    /// Called when the screen has been suspended.
    /// </summary>
    public virtual IEnumerator OnSuspend(GameObject prev, GameObject next)
    {
        Suspend?.Invoke(this, prev, next);
        yield break;
    }

    /// <summary>
    /// Called when the screen has been exited.
    /// </summary>
    public virtual IEnumerator OnExiting(GameObject prev, GameObject next)
    {
        Exiting?.Invoke(this, prev, next);
        yield break;
    }
}
