using System;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Party.Game.Camera;
using Party.Game.Vision;

namespace Party.Game.Testing;

public partial class DetectionTest : Node
{
    private TextureRect image;
    private VideoStreamPlayer video;
    private VideoFrameSource videoFrameSource;
    private ImageFrameSource imageFrameSource;

    public override void _Ready()
    {
        video = GetNode<VideoStreamPlayer>("%Video");
        image = GetNode<TextureRect>("%Image");

        imageFrameSource = new ImageFrameSource();
        videoFrameSource = new VideoFrameSource(video);

        GetNode<Button>("Start").Pressed += start;
        GetNode<Button>("Close").Pressed += close;

        GetNode<FileDialog>("Dialog_Video").FileSelected += onVideoSelected;
        GetNode<FileDialog>("Dialog_Image").FileSelected += onImageSelected;
    }

    public override void _ExitTree()
    {
        videoFrameSource.Dispose();
    }

    private void start()
    {
        if (VisionService.Current.Source == CameraService.Current)
        {
            return;
        }

        changeFrameSource(CameraService.Current);
        CameraService.Current.Start();
    }

    private void close()
    {
        if (VisionService.Current.Source != CameraService.Current)
        {
            return;
        }

        VisionService.Current.Source = null;
        CameraService.Current.Close();
    }

    private void onImageSelected(string path)
    {
        changeFrameSource(Image.LoadFromFile(path));
    }

    private void onVideoSelected(string path)
    {
        changeFrameSource(GD.Load<VideoStream>(path));
    }

    private void changeFrameSource<TCameraFeed>(TCameraFeed feed)
        where TCameraFeed : ICameraFeed, IFrameSource
    {
        video.Stop();
        video.Hide();
        image.Show();
        image.Texture = new CameraFeedTexture(feed);
        VisionService.Current.Source = feed;
    }

    private void changeFrameSource(Image resource)
    {
        video.Stop();
        video.Hide();
        image.Show();
    
        image.Texture = ImageTexture.CreateFromImage(resource);

        VisionService.Current.Source = imageFrameSource;
        imageFrameSource.Start(resource);
    }

    private void changeFrameSource(VideoStream resource)
    {
        video.Stop();
        video.Show();
        image.Hide();
        
        video.Stream = resource;
        VisionService.Current.Source = videoFrameSource;
        video.Play();
    }

    private class ImageFrameSource : IFrameSource
    {
        public bool Accelerated => false;
        public FrameSourceKind Kind => FrameSourceKind.Image;
        public event Action<Image> OnFrame;

        public void Start(Image image)
        {
            OnFrame?.Invoke(image);
        }
    }

    private class VideoFrameSource : IFrameSource, IDisposable
    {
        public bool Accelerated => false;
        public FrameSourceKind Kind => FrameSourceKind.Video;
        public event Action<Image> OnFrame;

        private readonly Task runner;
        private readonly CancellationTokenSource source;

        public VideoFrameSource(VideoStreamPlayer video)
        {
            source = new CancellationTokenSource();
            runner = Task.Factory
                .StartNew(() => poller(video, source.Token), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void Dispose()
        {
            source.Cancel();
            runner.Wait();
        }

        private void poller(VideoStreamPlayer video, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (!video.IsPlaying())
                {
                    continue;
                }

                var t = video.GetVideoTexture();
                var i = t.GetImage();

                OnFrame?.Invoke(i);
            }
        }
    }
}
