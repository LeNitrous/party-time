using System.Linq;
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

    public override void _Ready()
    {
        if (CameraService.Current is not null)
        {
            CameraService.Current.OnFeedAdded += onCameraFeedAdded;
            CameraService.Current.OnFeedRemoved += onCameraFeedRemoved;
            CameraService.Current.OnFrame += onCameraFrame;
            CameraService.Current.Start();
        }
        
        task = new GestureRecognizer();
        border = GetNode<Control>("%Indicator");
        camera = GetNode<TextureRect>("%Camera");
        camera.Texture = new CameraFeedTexture();

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

        task.Dispose();
    }

    private void onCameraFrame(Image image)
    {
        if (task is null)
        {
            return;
        }

        canDetectPresence = false;

        var result = task.Detect(image, FrameSource.Stream, CameraService.Current.Accelerated);
    
        for (int i = 0; i < result.Count; i++)
        {
            if (result[i].Gesture == Gesture.ThumbsUp)
            {
                canDetectPresence = true;
                break;
            }
        }

        hasResult = true;
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