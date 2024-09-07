using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class MenuScreen : Screen, IMoveHandler, ICancelHandler
{
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
                else if (pressed >= 0)
                {
                    tooltip.text = items[pressed].Description;
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

            invalidate = false;
        }
    }
}
