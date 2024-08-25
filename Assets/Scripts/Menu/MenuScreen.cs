using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
public class MenuScreen : MonoBehaviour, IPointerMoveHandler
{
    public string Title => title;

    public MenuScreenSelectedChanged OnSelect;

    [SerializeField]
    private string title;

    private int selected = -1;
    private MenuOption[] options;
    private GraphicRaycaster raycast;
    private List<RaycastResult> results;

    private void Awake()
    {
        if (options == null)
        {
            options = GetComponentsInChildren<MenuOption>();
        }

        if (raycast == null)
        {
            raycast = GetComponent<GraphicRaycaster>();
        }
    }

    private void OnEnable()
    {
        SetSelectedOption(0);
    }

    public void Next()
    {
        Step();
    }

    public void Previous()
    {
        Step(true);
    }

    private void Step(bool backward = false)
    {
        if (options == null || options.Length == 0)
        {
            return;
        }

        int current = selected + (backward ? -1 : 1);
        int minimum = 0;
        int maximum = options.Length - 1;

        if (current > maximum)
        {
            current = minimum;
        }

        if (current < minimum)
        {
            current = maximum;
        }

        SetSelectedOption(current);
    }

    private void SetSelectedOption(int index)
    {
        if (options == null || options.Length == 0)
        {
            return;
        }

        if (selected == index)
        {
            return;
        }

        var option = options[selected = index];

        OnSelect?.Invoke(option);
        EventSystem.current.SetSelectedGameObject(option.gameObject);
    }

    void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
    {
        results ??= new List<RaycastResult>();
        results.Clear();
        raycast.Raycast(eventData, results);

        if (results.Count <= 0)
        {
            return;
        }

        var hit = results[0].gameObject;

        if (!hit.TryGetComponent<MenuOption>(out var component))
        {
            return;
        }

        for (int index = 0; index < options.Length; index++)
        {
            if (options[index] == component)
            {
                SetSelectedOption(index);
                break;
            }
        }
    }
}

[Serializable]
public class MenuScreenSelectedChanged : UnityEvent<MenuOption>
{
}