using UnityEngine;
using UnityEngine.Events;

public class MenuItemAction : MenuItemHandler
{
    [Tooltip("The action performed when the menu item is selected.")]
    public UnityEvent Action;

    public override void Confirm()
    {
        Action?.Invoke();
    }
}
