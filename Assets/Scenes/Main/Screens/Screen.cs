using System.Collections;
using UnityEngine;

public class Screen : MonoBehaviour
{
    /// <summary>
    /// Called when the screen has been entered.
    /// </summary>
    public virtual IEnumerator OnScreenEntered(GameObject prev, GameObject next)
    {
        yield return null;
    }

    /// <summary>
    /// Called when the screen has been resumed.
    /// </summary>
    public virtual IEnumerator OnScreenResumed(GameObject prev, GameObject next)
    {
        yield return null;
    }

    /// <summary>
    /// Called when the screen has been suspended.
    /// </summary>
    public virtual IEnumerator OnScreenSuspend(GameObject prev, GameObject next)
    {
        yield return null;
    }

    /// <summary>
    /// Called when the screen has been exited.
    /// </summary>
    public virtual IEnumerator OnScreenExited(GameObject prev, GameObject next)
    {
        yield return null;
    }
}
