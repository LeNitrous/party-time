using Godot;
using Party.Game.Input;

namespace Party.Game.Menu;

public partial class TooltipController : Label
{
    private HoverFocusListener hoverFocusListener;

    public override void _Ready()
    {
        hoverFocusListener = new HoverFocusListener(hoverFocusChanged);
    }

    public override void _ExitTree()
    {
        hoverFocusListener?.Dispose();
    }

    private void hoverFocusChanged(NodePath path)
    {
        Text = path is null || path.IsEmpty ? string.Empty : Tr(GetNode(path).GetMeta("tooltip", string.Empty).AsString());
    }
}
