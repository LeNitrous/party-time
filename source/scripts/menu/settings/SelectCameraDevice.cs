using System.Linq;
using System.Threading;
using GDExtension.Wrappers;
using Godot;
using Party.Game.Camera;
using Party.Game.Detection;
using Party.Game.Menu;

public sealed partial class SelectCameraDevice : Node
{
    private GestureRecognizer task;
    private Choice choice;
    private TextureRect camera;
    private Control border;
    private bool hasResult;
    private bool canDetectPresence;
    private bool hasUpdatedCameras;
    private readonly ManualResetEventSlim reset = new ManualResetEventSlim(false);

    public override void _Ready()
    {
        task = new GestureRecognizer();

        if (CameraService.Current is not null)
        {
            CameraService.Current.OnFeedAdded += onCameraFeedAdded;
            CameraService.Current.OnFeedRemoved += onCameraFeedRemoved;
            CameraService.Current.OnFrame += onCameraFrame;
            CameraService.Current.Start();
        }

        border = GetNode<Control>("%Indicator");
        camera = GetNode<TextureRect>("%Camera");

        choice = GetNode<Choice>("%Select");
        choice.SelectionChanged += onCameraFeedSelectionChanged;

        onCameraFeedCollectionChanged();
    }

    public override void _Process(double delta)
    {
        if (hasResult)
        {
            border.Visible = canDetectPresence;
            hasResult = false;
        }

        if (hasUpdatedCameras)
        {
            if (CameraService.Current is not null)
            {
                choice.Options = new Godot.Collections.Array<string>(CameraService.Current.Feeds.Select(f => f.Name).ToArray());
            }

            hasUpdatedCameras = false;
        }
    }

    public override void _ExitTree()
    {
        if (CameraService.Current is not null)
        {
            CameraService.Current.OnFeedAdded -= onCameraFeedAdded;
            CameraService.Current.OnFeedRemoved -= onCameraFeedRemoved;
            CameraService.Current.OnFrame -= onCameraFrame;
            CameraService.Current.Close();
        }

        reset.Wait();

        task.Dispose();
        task = null;
    }

    private void onCameraFrame(MediaPipeImage mp)
    {
        if (task is null)
        {
            return;
        }

        reset.Reset();

        canDetectPresence = false;

        if (mp.IsGpuImage())
        {
            mp.ConvertToCpu();
        }

        var result = task.Detect(mp, FrameSource.Stream);
    
        for (int i = 0; i < result.Count; i++)
        {
            if (result[i].Gesture == Gesture.ThumbsUp)
            {
                canDetectPresence = true;
                break;
            }
        }

        using var image = mp.GetImage();

        if (camera.Texture.GetSize() == image.GetSize())
        {
            camera.Texture.CallDeferred(ImageTexture.MethodName.Update, image);
        }
        else
        {
            camera.Texture.CallDeferred(ImageTexture.MethodName.SetImage, image);
        }

        hasResult = true;

        reset.Set();

        System.GC.Collect(System.GC.MaxGeneration, System.GCCollectionMode.Forced);
        System.GC.WaitForPendingFinalizers();
    }

    private void onCameraFeedSelectionChanged(int index)
    {
        if (CameraService.Current is not null)
        {
            CameraService.Current.Feed = index;
        }
    }

    private void onCameraFeedCollectionChanged()
    {
        hasUpdatedCameras = true;
    }

    private void onCameraFeedAdded(Party.Game.Camera.CameraFeed feed)
    {
        onCameraFeedCollectionChanged();
    }

    private void onCameraFeedRemoved(Party.Game.Camera.CameraFeed feed)
    {
        onCameraFeedCollectionChanged();
    }
}