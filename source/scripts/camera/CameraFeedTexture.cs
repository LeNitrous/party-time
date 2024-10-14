using Godot;

namespace Party.Game.Camera;

public sealed partial class CameraFeedTexture : Texture2D
{
    private Rid texture;
    private Rid placeholder;
    private readonly ICameraFeed feed;

    public CameraFeedTexture()
        : this(CameraService.Current)
    {
    }

    public CameraFeedTexture(ICameraFeed feed)
    {
        this.feed = feed;
        this.feed.OnClose += onClose;
        this.feed.OnStart += onStart;
    }

    public override int _GetWidth()
    {
        return feed.Width;
    }

    public override int _GetHeight()
    {
        return feed.Height;
    }

    public override Rid _GetRid()
    {
        if (texture.IsValid)
        {
            return texture;
        }

        if (!placeholder.IsValid)
        {
            placeholder = RenderingServer.Texture2DPlaceholderCreate();
        }

        return placeholder;
    }

    protected override void Dispose(bool disposing)
    {
        RenderingServer.FreeRid(placeholder);
        placeholder = default;
        feed.OnClose -= onClose;
        feed.OnStart -= onStart;
        base.Dispose(disposing);
    }

    private void onStart()
    {
        CallDeferred(Resource.MethodName.EmitChanged);
    }

    private void onClose()
    {
        RenderingServer.FreeRid(texture);
        texture = default;
        CallDeferred(Resource.MethodName.EmitChanged);
    }
}
