using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlashCap;
using Godot;

namespace Party.Game.Camera;

public sealed class CameraServiceDesktop : CameraService.Impl
{
    private readonly Task runner;
    private readonly CancellationTokenSource source;

    public CameraServiceDesktop(CameraService owner)
    {
        source = new CancellationTokenSource();
        runner = Task.Factory
            .StartNew(() => devicePoll(owner, source.Token), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
    }

    public override bool PermissionGranted()
    {
        return true;
    }

    public override Task<bool> RequestPermission()
    {
        return Task.FromResult(true);
    }

    public override void Dispose()
    {
        source.Cancel();
        runner.Wait();
    }

    private void devicePoll(CameraService owner, CancellationToken token)
    {
        var current = new Dictionary<string, CameraFeed>();
        var removed = new List<string>();
        var devices = new CaptureDevices();

        while (!token.IsCancellationRequested)
        {
            var descriptors = devices.EnumerateDescriptors();

            removed.Clear();
            removed.AddRange(current.Keys.Except(descriptors.Select(d => d.Name)));

            foreach (string device in removed)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                if (current.Remove(device, out var feed))
                {
                    owner.RemoveFeed(feed);
                }
            }

            foreach (var descriptor in descriptors)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                if (descriptor.Characteristics.Length <= 0)
                {
                    continue;
                }

                if (current.TryAdd(descriptor.Name, new CameraFeedDesktop(descriptor, descriptor.Characteristics[0])))
                {
                    owner.AddFeed(current[descriptor.Name]);
                }
            }

            Thread.Sleep(1000);
        }
    }

    private sealed class CameraFeedDesktop : CameraFeed
    {
        public override string Name => descriptor.Name;
        public override int Width => characters.Width;
        public override int Height => characters.Height;
        public override bool Accelerated => false;

        private Task runner;
        private CancellationTokenSource source;
        private readonly VideoCharacteristics characters;
        private readonly CaptureDeviceDescriptor descriptor;

        public CameraFeedDesktop(CaptureDeviceDescriptor descriptor, VideoCharacteristics characters)
        {
            this.descriptor = descriptor;
            this.characters = characters;
        }

        public override void Start()
        {
            if (source is not null || runner is not null)
            {
                return;
            }

            source = new CancellationTokenSource();
            runner = Task.Factory
                .StartNew(() => thread(source.Token), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public override void Close()
        {
            if (source is null || runner is null)
            {
                return;
            }

            source.Cancel();
            source = null;

            runner.Wait();
            runner = null;
        }

        private async Task thread(CancellationToken token)
        {
            using var observer = new Observer(this, characters);
            using var observable = await descriptor.AsObservableAsync(characters, token);
            using var subscription = observable.Subscribe(observer);

            await observable.StartAsync(CancellationToken.None);

            EmitStart();

            token.WaitHandle.WaitOne();

            EmitClose();

            await observable.StopAsync(CancellationToken.None);
        }

        private class Observer : IObserver<PixelBufferScope>, IDisposable
        {
            private bool disposed;
            private bool isDisposeRequested;
            private byte[] buffer;
            private Image image;
            private readonly CameraFeedDesktop owner;
            private readonly VideoCharacteristics video;

            public Observer(CameraFeedDesktop owner, VideoCharacteristics video)
            {
                this.owner = owner;
                this.video = video;
            }

            public void OnNext(PixelBufferScope value)
            {
                if (!isDisposeRequested)
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

                    owner.EmitFrame(image);
                }
                else
                {
                    image.Dispose();
                    image = null;

                    buffer = null;
                }
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

                isDisposeRequested = true;

                disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}