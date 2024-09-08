using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MenuItemHandler : MonoBehaviour, ISubmitHandler, IPointerClickHandler
{
    /// <summary>
    /// The label displayed in hints.
    /// </summary>
    public string Label;

    /// <summary>
    /// Confirms selection for this menu item.
    /// </summary>
    public abstract void Confirm();

    void ISubmitHandler.OnSubmit(BaseEventData eventData)
    {
        Confirm();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (!eventData.eligibleForClick)
        {
            return;
        }

        if (eventData.pointerPressRaycast.gameObject == gameObject)
        {
            Confirm();
        }
    }
}
