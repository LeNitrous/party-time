using Godot;

namespace Party.Game.Experience;

public partial class ControllerStatusLabel : Label
{
    private Controller controller;

    public override void _Ready()
    {
        controller = GetNode<Controller>("%Controller");
        controller.Connect(Controller.SignalName.ActionPerformed, Callable.From<int>(_ => update()));
        controller.Connect(Controller.SignalName.PositionChanged, Callable.From<Vector2>(_ => update()));
        update();
    }

    private void update()
    {
        Text = string.Format(format, controller.Current, controller.Position);
    }

    private static readonly string format = "Action: {0}\nPosition: {1}";
}
