using UnityEngine;

public class MenuItemOpen : MenuItemHandler
{
    [SerializeField]
    private GameObject screen;

    private ScreenController controller;

    public override void Confirm()
    {
        if (controller != null && screen != null)
        {
            controller.Push(screen);
        }
    }

    private void Start()
    {
        controller = GetComponentInParent<ScreenController>();
    }
}
