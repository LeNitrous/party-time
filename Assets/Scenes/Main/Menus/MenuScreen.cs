using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class MenuScreen : Screen, IMoveHandler, ICancelHandler
{
    [Flags]
    private enum HintFlags : byte
    {
        None = 0,
        Back = 1 << 0,
        Submit = 1 << 1,
        Click = 1 << 2,
        Navigate = 1 << 3,
    }

    /// <summary>
    /// Called when the menu item selection has been changed.
    /// </summary>
    public event Action<MenuScreen, MenuItem> OnSelectionChanged;

    private bool invalidate = true;
    private int hovered = -1;
    private int current = -1;
    private int pressed = -1;
    private MenuItem[] items;
    private TextMeshProUGUI tooltip;
    private MenuItemBack back;
    private HintController hints;
    private HintFlags currHintFlags;
    private List<HintController.Control> controls;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Select(int index)
    {
        if (items == null || items.Length == 0)
        {
            throw new InvalidOperationException();
        }

        if (index < 0 || index >= items.Length)
        {
            throw new ArgumentOutOfRangeException();
        }

        var item = items[index];

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(item.gameObject);
        }

        current = index;

        OnSelectionChanged?.Invoke(this, item);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Next()
    {
        Step(+1);
    }

    /// <summary>
    /// 
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

        int next;

        if (current < 0)
        {
            next = 0;
        }
        else
        {
            next = current + direction;
        }

        if (next >= items.Length)
        {
            next = 0;
        }

        if (next < 0)
        {
            next = items.Length - 1;
        }

        Select(next);
    }

    private void OnItemHovered(MenuItem item)
    {
        hovered = Array.IndexOf(items, item);
        invalidate = true;
        
        if (EventSystem.current != null)
        {
            current = -1;
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    private void OnItemHoveredLost(MenuItem item)
    {
        int index = Array.IndexOf(items, item);

        if (index != hovered)
        {
            return;
        }

        hovered = -1;
        invalidate = true;
    }

    private void OnItemPressed(MenuItem item)
    {
        pressed = Array.IndexOf(items, item);
        invalidate = true;
    }

    private void OnItemReleased(MenuItem item)
    {
        pressed = -1;
        invalidate = true;
    }

    private void OnItemSelected(MenuItem item)
    {
        current = Array.IndexOf(items, item);
        invalidate = true;
    }

    void IMoveHandler.OnMove(AxisEventData data)
    {
        switch (data.moveDir)
        {
            case MoveDirection.Up:
                Previous();
                break;

            case MoveDirection.Down:
                Next();
                break;
        }
    }

    void ICancelHandler.OnCancel(BaseEventData eventData)
    {
        if (back != null)
        {
            back.Confirm();
        }
    }

    private void Start()
    {
        hints = FindFirstObjectByType<HintController>();
        items = transform.Find("Content").GetComponentsInChildren<MenuItem>();
        
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            
            if (item != null)
            {
                item.OnHover += OnItemHovered;
                item.OnHoverLost += OnItemHoveredLost;
                item.OnSelect += OnItemSelected;
                item.OnPressed += OnItemPressed;
                item.OnReleased += OnItemReleased;
            }

            item.TryGetComponent(out back);
        }
        
        tooltip = transform.Find("Footer/Description").GetComponent<TextMeshProUGUI>();
    }

    private void OnDisable()
    {
        hovered = -1;
        pressed = -1;
        current = -1;
        invalidate = true;
    }

    private void Update()
    {
        if (invalidate)
        {
            if (tooltip != null)
            {
                if (current >= 0)
                {
                    tooltip.text = items[current].Description;
                }
                else if (hovered >= 0)
                {
                    tooltip.text = items[hovered].Description;
                }
                else
                {
                    tooltip.text = string.Empty;
                }
            }

            if (hints != null)
            {
                var nextHintFlags = HintFlags.None;

                if (back != null)
                {
                    nextHintFlags |= HintFlags.Back;
                }

                if (current >= 0)
                {
                    nextHintFlags |= HintFlags.Navigate;

                    if (items[current].TryGetComponent<MenuItemHandler>(out _))
                    {
                        nextHintFlags |= HintFlags.Submit;
                    }
                }
                else if (hovered >= 0)
                {
                    if (items[hovered].TryGetComponent<MenuItemHandler>(out _))
                    {
                        nextHintFlags |= HintFlags.Click;
                    }
                }

                if (currHintFlags != nextHintFlags)
                {
                    controls ??= new List<HintController.Control>();
                    controls.Clear();

                    if (nextHintFlags.HasFlag(HintFlags.Back))
                    {
                        controls.Add(new HintController.Control("Back", "Cancel"));
                    }

                    if (nextHintFlags.HasFlag(HintFlags.Click))
                    {
                        if (items[hovered].TryGetComponent<MenuItemHandler>(out var handler))
                        {
                            controls.Add(new HintController.Control(string.IsNullOrEmpty(handler.Label) ? "Confirm" : handler.Label, "Click"));
                        }
                    }

                    if (nextHintFlags.HasFlag(HintFlags.Navigate))
                    {
                        controls.Add(new HintController.Control("Select", "Navigate"));
                    }

                    if (nextHintFlags.HasFlag(HintFlags.Submit))
                    {
                        if (items[current].TryGetComponent<MenuItemHandler>(out var handler))
                        {
                            controls.Add(new HintController.Control(string.IsNullOrEmpty(handler.Label) ? "Confirm" : handler.Label, "Submit"));
                        }
                    }

                    hints.Hints = controls.ToArray();
                    currHintFlags = nextHintFlags;
                }
            }

            invalidate = false;
        }
    }
}
