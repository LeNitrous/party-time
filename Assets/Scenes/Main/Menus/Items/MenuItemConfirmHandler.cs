using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class MenuItemConfirmHandler : MonoBehaviour, ISubmitHandler, IPointerClickHandler
{
    private GraphicRaycaster caster;
    private List<RaycastResult> results;

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
        results ??= new List<RaycastResult>();
        results.Clear();

        if (caster == null)
        {
            caster = GetComponentInParent<GraphicRaycaster>();
        }

        caster.Raycast(eventData, results);

        var first = results[0].gameObject;

        if (first == gameObject)
        {
            Confirm();
        }
    }
}
