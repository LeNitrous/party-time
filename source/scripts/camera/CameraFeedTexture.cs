using Godot;

namespace Party.Game.Camera;

public sealed partial class CameraFeedTexture : Texture2D
{
    private Rid texture;
    private Rid placeholder;
    private readonly ICameraFeed feed;

    public CameraFeedTexture(ICameraFeed feed)
    {
        this.feed = feed;
        this.feed.OnFrame += onFrame;
        this.feed.OnClose += onClose;
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
        feed.OnFrame -= onFrame;
        feed.OnClose -= onClose;
        base.Dispose(disposing);
    }

    private void onClose()
    {
        RenderingServer.FreeRid(texture);
        texture = default;
        CallDeferred(Resource.MethodName.EmitChanged);
    }

    private void onFrame(Image image)
    {
        if (!texture.IsValid)
        {
            texture = RenderingServer.Texture2DCreate(image);
        }
        else
        {
            RenderingServer.Texture2DUpdate(texture, image, 0);
        }

        CallDeferred(Resource.MethodName.EmitChanged);
    }
}
