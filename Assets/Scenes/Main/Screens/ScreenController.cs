using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenController : MonoBehaviour
{
    [SerializeField, Tooltip("The initial screen to be shown.")]
    private GameObject initial;

    private Stack<GameObject> stack;

    public void Push(GameObject next)
    {
        if (next == null)
        {
            throw new ArgumentNullException();
        }

        stack ??= new Stack<GameObject>();
        stack.TryPeek(out var prev);
        StartCoroutine(Push(prev, next));
        next.SetActive(true);
    }

    public void Pop()
    {
        if (stack == null)
        {
            return;
        }

        stack.TryPop(out var prev);
        stack.TryPeek(out var next);
        StartCoroutine(Pop(prev, next));
    }

    private IEnumerator Push(GameObject prev, GameObject next)
    {
        if (prev != null)
        {
            if (prev.TryGetComponent<Screen>(out var screen))
            {
                yield return screen.OnScreenSuspend(prev, next);
            }
            
            prev.SetActive(false);
        }

        if (next != null)
        {
            next = Instantiate(next, transform);
            next.name = next.name.Replace("(Clone)", string.Empty);
            stack.Push(next);

            if (next.TryGetComponent<Screen>(out var screen))
            {
                yield return screen.OnScreenEntered(prev, next);
                
                if (EventSystem.current != null)
                {
                    EventSystem.current.SetSelectedGameObject(next);
                }
            }
        }
    }

    private IEnumerator Pop(GameObject prev, GameObject next)
    {
        if (prev != null)
        {
            if (prev.TryGetComponent<Screen>(out var screen))
            {
                yield return screen.OnScreenExited(prev, next);
            }

            Destroy(prev);
        }

        if (next != null)
        {
            next.SetActive(true);

            if (next.TryGetComponent<Screen>(out var screen))
            {
                yield return screen.OnScreenResumed(prev, next);
            }

            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(next);
            }
        }
    }

    private void Start()
    {
        if (initial != null)
        {
            Push(initial);
        }
    }
}
