using System.Threading;
using System.Threading.Tasks;
using GDExtension.Wrappers;
using Godot;
using Party.Game.Camera;

namespace Party.Game.Detection;

public abstract partial class DetectionTest<TAnnotator, TOutput> : Control
    where TAnnotator : DetectionAnnotator<TOutput>
{
    private DetectionTask<TOutput> task;
    private TAnnotator annotate;
    private TextureRect image;
    private VideoStreamPlayer video;
    private Task runner;
    private CancellationTokenSource source;

    public override void _Ready()
    {
        task = CreateTask();

        image = GetNode<TextureRect>("%Image");
        video = GetNode<VideoStreamPlayer>("%Video");
        annotate = GetNode<TAnnotator>("%Annotator");

        GetNode<Button>("%Start").Pressed += start;
        GetNode<Button>("%Close").Pressed += close;

        GetNode<FileDialog>("%Dialog_Video").FileSelected += onVideoSelected;
        GetNode<FileDialog>("%Dialog_Image").FileSelected += onImageSelected;

        source = new CancellationTokenSource();
        runner = Task.Factory
            .StartNew(() => pollForStream(source.Token), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    public override void _ExitTree()
    {
        source.Cancel();
        source = null;

        runner.Wait();
        runner = null;

        task?.Dispose();
        task = null;
    }

    protected abstract DetectionTask<TOutput> CreateTask();

    private void start()
    {
        video.Stop();
        video.Hide();
        image.Show();
        CameraService.Current.OnFrame += onCameraFrame;
        CameraService.Current.Start();
    }

    private void close()
    {
        CameraService.Current.OnFrame -= onCameraFrame;
        CameraService.Current.Close();
    }

    private void onImageSelected(string path)
    {
        var resource = Image.LoadFromFile(path);
        video.Stop();
        video.Hide();
        image.Show();
        image.Texture = ImageTexture.CreateFromImage(resource);
        annotate.Annotate(task.Detect(MediaPipeImage.CreateFromImage(resource), FrameSource.Image));
    }

    private void onVideoSelected(string path)
    {
        image.Hide();
        video.Stop();
        video.Show();
        video.Stream = GD.Load<VideoStream>(path);
        video.Play();
    }

    private void onCameraFrame(MediaPipeImage mp)
    {
        annotate.Annotate(task.Detect(mp, FrameSource.Stream));

        if (mp.IsGpuImage())
        {
            mp.ConvertToCpu();
        }

        if (image.Texture is null)
        {
            image.CallDeferred(TextureRect.MethodName.SetTexture, ImageTexture.CreateFromImage(mp.GetImage()));
        }
        else
        {
            image.Texture.CallDeferred(ImageTexture.MethodName.Update, mp.GetImage());
        }
    }

    private void pollForStream(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (video.Paused || !video.IsPlaying())
            {
                continue;
            }

            var t = video.GetVideoTexture();
            var i = t.GetImage();

            annotate.Annotate(task.Detect(MediaPipeImage.CreateFromImage(i), FrameSource.Video));
            image.CallThreadSafe(TextureRect.MethodName.SetTexture, t);
        }
    }
}
