using Godot;

namespace Party.Game.Detection;

public abstract partial class DetectionAnnotator<TOutput> : Control
{
    private bool isDrawRequested;
    private TOutput data;

    public override void _Draw()
    {
        if (isDrawRequested)
        {
            Render(data);
            data = default;
            isDrawRequested = false;
        }
    }

    public void Annotate(TOutput output)
    {
        data = output;
        isDrawRequested = true;
        CallDeferred(CanvasItem.MethodName.QueueRedraw);
    }

    protected abstract void Render(TOutput output);
}
