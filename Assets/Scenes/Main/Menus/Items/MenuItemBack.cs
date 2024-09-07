public class MenuItemBack : MenuItemConfirmHandler
{
    private ScreenController controller;

    public override void Confirm()
    {
        if (controller != null)
        {
            controller.Pop();
        }
    }

    private void Start()
    {
        controller = GetComponentInParent<ScreenController>();
    }
}
