using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsScreen : Screen, ICancelHandler
{
    private HintController hints;

    public override IEnumerator OnEntered(GameObject prev, GameObject next)
    {
        if (hints == null)
        {
            hints = FindFirstObjectByType<HintController>();
        }

        if (hints != null)
        {
            hints.Hints = back;
        }

        return base.OnEntered(prev, next);
    }

    public override IEnumerator OnExiting(GameObject prev, GameObject next)
    {
        if (hints != null)
        {
            hints.Hints = Array.Empty<HintController.Control>();
        }

        return base.OnExiting(prev, next);
    }

    void ICancelHandler.OnCancel(BaseEventData eventData)
    {
        GetComponentInParent<ScreenController>().Exit();
    }

    private void Update()
    {
        if (EventSystem.current != null)
        {
            if (EventSystem.current.currentSelectedGameObject != gameObject)
            {
                // HACK: Eagerly take control of the event system or else we cannot exit the screen!
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
        }
    }

    private static readonly HintController.Control[] back = new HintController.Control[] { new HintController.Control("Back", "Cancel") };
}
