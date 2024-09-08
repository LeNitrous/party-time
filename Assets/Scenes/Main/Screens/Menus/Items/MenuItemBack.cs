public class MenuItemBack : MenuItemHandler
{
    private ScreenController controller;

    public override void Confirm()
    {
        if (controller != null)
        {
            controller.Exit();
        }
    }

    private void Start()
    {
        controller = GetComponentInParent<ScreenController>();
    }
}
