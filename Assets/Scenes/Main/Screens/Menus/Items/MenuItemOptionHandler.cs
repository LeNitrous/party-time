using System;
using UnityEngine;

public abstract class MenuItemOptionHandler : MonoBehaviour
{
    private MenuItemOption option;

    public void Reload()
    {
        if (option != null)
        {
            if (option.Options == null || option.Options.Length == 0)
            {
                option.Options = GetOptions();
            }

            if (HasDefault())
            {
                option.Current = GetDefault();
            }
        }
    }

    protected virtual string[] GetOptions()
    {
        return Array.Empty<string>();
    }

    protected virtual bool HasDefault()
    {
        return true;
    }

    protected abstract int GetDefault();

    protected abstract void SelectionChanged(int index, string value);

    private void OnSelectionChanged(MenuItemOption sender)
    {
        SelectionChanged(sender.Current, sender.Options[sender.Current]);
    }

    private void Start()
    {
        Reload();
    }

    private void OnEnable()
    {
        if (option == null)
        {
            option = GetComponent<MenuItemOption>();
        }

        if (option != null)
        {
            option.OnSelectionChanged += OnSelectionChanged;
        }
    }

    private void OnDisable()
    {
        if (option != null)
        {
            option.OnSelectionChanged -= OnSelectionChanged;
        }
    }
}
