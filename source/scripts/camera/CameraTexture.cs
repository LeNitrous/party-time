using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlashCap;
using Godot;

namespace Party.Game.Camera;

[GlobalClass, Tool]
public partial class CameraTexture : Texture2D
{
    private Task runner;
    private CancellationTokenSource source;

    public void Start()
    {
        source = new CancellationTokenSource();
        runner = Task.Factory
            .StartNew(() => start(source.Token), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    public void Stop()
    {
        source.Cancel();
        source = null;

        runner.Wait();
        runner = null;
    }

    private async Task start(CancellationToken token)
    {
        var descriptor = new CaptureDevices().EnumerateDescriptors().ElementAt(0);
        var characters = descriptor.Characteristics[0];

        using var observer = new Observer(this, characters);
        using var observable = await descriptor.AsObservableAsync(characters, token);
        using var subscription = observable.Subscribe(observer);

        await observable.StartAsync(token);

        token.WaitHandle.WaitOne();

        await observable.StopAsync(CancellationToken.None);
    }

    private class Observer : IObserver<PixelBufferScope>, IDisposable
    {
        private bool disposed;
        private byte[] buffer;
        private Image image;
        private Texture2D owner;
        private readonly VideoCharacteristics video;

        public Observer(Texture2D owner, VideoCharacteristics video)
        {
            this.owner = owner;
            this.video = video;
        }

        public void OnNext(PixelBufferScope value)
        {
            if (buffer is null)
            {
                buffer = value.Buffer.CopyImage();
            }
            else
            {
                var referred = value.Buffer.ReferImage();

                if (buffer.Length < referred.Count)
                {
                    Array.Resize(ref buffer, referred.Count);
                }

                referred.CopyTo(buffer);
            }

            image ??= new Image();

            switch (video.PixelFormat)
            {
                case PixelFormats.PNG:
                    image.LoadPngFromBuffer(buffer);
                    break;

                case PixelFormats.JPEG:
                    image.LoadJpgFromBuffer(buffer);
                    break;

                case PixelFormats.RGB8:
                    image.SetData(video.Width, video.Height, false, Image.Format.Rgb8, buffer);
                    break;
            }

            image.FlipX();

            RenderingServer.Texture2DUpdate(owner.GetRid(), image, 0);
        }

        public void OnCompleted()
        {
            Dispose();
        }

        public void OnError(Exception error)
        {
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            image.Dispose();
            image = null;

            buffer = null;

            disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
