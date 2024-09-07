using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuItemOptions : MonoBehaviour, ISelectHandler, IDeselectHandler, IMoveHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Called when the menu item's current selection has been changed.
    /// </summary>
    public event Action<MenuItemOptions> OnSelectionChanged;

    /// <summary>
    /// An array of selectable options.
    /// </summary>
    public string[] Options
    {
        get => options;
        set
        {
            options = value;
            dirty = true;
        }
    }

    /// <summary>
    /// The current selected value's index.
    /// </summary>
    public int Current
    {
        get => current;
        set
        {
            if (current == value)
            {
                return;
            }

            current = value;
            dirty = true;
        }
    }

    [SerializeField]
    private string[] options;

    [SerializeField]
    private int current;

    private bool dirty = true;
    private bool hovered;
    private bool selected;
    private Transform caretL;
    private Transform caretR;
    private TextMeshProUGUI option;
    private GraphicRaycaster caster;
    private List<RaycastResult> results;

    private void Start()
    {
        option = transform.Find("Options/Label").GetComponent<TextMeshProUGUI>();
        caretL = transform.Find("Options/Left");
        caretR = transform.Find("Options/Right");
        caster = GetComponentInParent<GraphicRaycaster>();
    }

    /// <summary>
    /// Changes the selection to the specified index.
    /// </summary>
    /// <param name="index">The index of the item to choose.</param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Select(int index)
    {
        if (Options == null || Options.Length == 0)
        {
            throw new InvalidOperationException();
        }

        if (index < 0 || index >= Options.Length)
        {
            throw new ArgumentOutOfRangeException();
        }

        option.text = Options[current = index];
        OnSelectionChanged?.Invoke(this);
    }

    /// <summary>
    /// Selects the next item.
    /// </summary>
    public void Next()
    {
        Step(+1);
    }

    /// <summary>
    /// Selects the previous item.
    /// </summary>
    public void Previous()
    {
        Step(-1);
    }

    private void Step(int direction)
    {
        if (direction == 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        int next = current - direction;

        if (next >= Options.Length)
        {
            next = 0;
        }

        if (next < 0)
        {
            next = Options.Length - 1;
        }

        Select(next);
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        dirty = true;
        selected = true;
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        dirty = true;
        selected = false;
    }

    void IMoveHandler.OnMove(AxisEventData data)
    {
        switch (data.moveDir)
        {
            case MoveDirection.Left:
                Previous();
                return;

            case MoveDirection.Right:
                Next();
                return;
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        dirty = true;
        hovered = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        dirty = true;
        hovered = false;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        results ??= new List<RaycastResult>();
        results.Clear();

        caster.Raycast(eventData, results);

        var first = results[0].gameObject;

        if (first == caretL.gameObject)
        {
            Next();
        }

        if (first == caretR.gameObject)
        {
            Previous();
        }
    }

    private void Update()
    {
        if (dirty)
        {
            if (Options == null || Options.Length <= 0)
            {
                if (option != null)
                {
                    option.gameObject.SetActive(false);
                }

                if (caretL != null)
                {
                    caretL.gameObject.SetActive(false);
                }

                if (caretR != null)
                {
                    caretR.gameObject.SetActive(false);
                }
            }
            else
            {
                if (option != null)
                {
                    option.gameObject.SetActive(true);
                    option.text = Options[current = Mathf.Clamp(current, 0, Options.Length - 1)];
                }

                if (caretL != null)
                {
                    caretL.gameObject.SetActive(hovered || selected);
                }

                if (caretR != null)
                {
                    caretR.gameObject.SetActive(hovered || selected);
                }
            }

            if (option != null)
            {
                option.color = hovered || selected ? Color.white : black;
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        dirty = true;
    }
#endif

    private static readonly Color black = new Color(22 / 255.0f, 22 / 255.0f, 22 / 255.0f);
}
