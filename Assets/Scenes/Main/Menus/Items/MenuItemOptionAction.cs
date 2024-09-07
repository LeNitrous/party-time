using System;
using UnityEngine;
using UnityEngine.Events;

public class MenuItemOptionAction : MenuItemOptionHandler
{
    [SerializeField]
    private MenuItemSelectionChangedEvent selectionChanged;

    protected override int GetDefault()
    {
        return 0;
    }

    protected override bool HasDefault()
    {
        return false;
    }

    protected override void SelectionChanged(int index, string value)
    {
        selectionChanged?.Invoke(new MenuItemSelectionChangedArgs(index, value));
    }
}

public struct MenuItemSelectionChangedArgs
{
    public int Index;
    public string Value;

    public MenuItemSelectionChangedArgs(int index, string value)
    {
        Index = index;
        Value = value;
    }
}

[Serializable]
public class MenuItemSelectionChangedEvent : UnityEvent<MenuItemSelectionChangedArgs>
{
}
