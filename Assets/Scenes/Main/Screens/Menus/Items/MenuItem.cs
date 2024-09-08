using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MenuItem : MonoBehaviour, IMoveHandler, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    /// <summary>
    /// Called when the menu item has been selected.
    /// </summary>
    public event Action<MenuItem> OnSelect;

    /// <summary>
    /// Called when the menu item has been deselected.
    /// </summary>
    public event Action<MenuItem> OnDeselect;

    /// <summary>
    /// Called when the menu item has been hovered.
    /// </summary>
    public event Action<MenuItem> OnHover;

    /// <summary>
    /// Called when the menu item has lost hover.
    /// </summary>
    public event Action<MenuItem> OnHoverLost;

    /// <summary>
    /// Called when the menu item has been pressed.
    /// </summary>
    public event Action<MenuItem> OnPressed;

    /// <summary>
    /// Called when the menu item has been released.
    /// </summary>
    public event Action<MenuItem> OnReleased;

    /// <summary>
    /// The flavor text for this menu item.
    /// </summary>
    public string Description;

    private bool dirty = true;
    private bool pressed = false;
    private bool hovered = false;
    private bool selected = false;
    private Image filled;
    private MenuScreen screen;
    private TextMeshProUGUI tagged;
    private GraphicRaycaster caster;
    private List<RaycastResult> results;

    void IMoveHandler.OnMove(AxisEventData data)
    {
        switch (data.moveDir)
        {
            case MoveDirection.Up when screen != null:
                screen.Previous();
                return;

            case MoveDirection.Down when screen != null:
                screen.Next();
                return;
        }
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        if (!selected)
        {
            dirty = true;
            selected = true;
            OnSelect?.Invoke(this);
        }
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        if (selected)
        {
            dirty = true;
            selected = false;
            OnDeselect?.Invoke(this);
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (!hovered)
        {
            dirty = true;
            hovered = true;
            OnHover?.Invoke(this);
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (hovered)
        {
            dirty = true;
            hovered = false;
            OnHoverLost?.Invoke(this);
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        results ??= new List<RaycastResult>();
        results.Clear();

        caster.Raycast(eventData, results);

        if (results[0].gameObject == gameObject)
        {
            if (!pressed)
            {
                dirty = true;
                pressed = true;
                OnPressed?.Invoke(this);
            }
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (pressed)
        {
            dirty = true;
            pressed = false;
            OnReleased?.Invoke(this);
        }
    }

    private void Start()
    {
        tagged = transform.Find("Label").GetComponent<TextMeshProUGUI>();
        filled = GetComponent<Image>();
        screen = GetComponentInParent<MenuScreen>();
        caster = GetComponentInParent<GraphicRaycaster>();
    }

    private void OnDisable()
    {
        dirty = true;
        pressed = false;
        hovered = false;
        selected = false;
    }

    private void Update()
    {
        if (dirty)
        {

            if (filled != null)
            {
                if (pressed)
                {
                    filled.color = new Color(28 / 255.0f, 28 / 255.0f, 28 / 255.0f);
                }
                else
                {
                    filled.color = hovered || selected ? black : default;
                }
            }

            if (tagged != null)
            {
                tagged.color = hovered || selected || pressed ? Color.white : black;
            }

            dirty = false;
        }

#if UNITY_EDITOR
        if (tagged != null && tagged.text != name)
        {
            tagged.text = name;
        }
#endif
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        dirty = true;
    }
#endif

    private static readonly Color black = new Color(22 / 255.0f, 22 / 255.0f, 22 / 255.0f);
}
