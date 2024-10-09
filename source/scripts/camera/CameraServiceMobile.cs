using System.Threading.Tasks;
using GDExtension.Wrappers;
using Godot;

namespace Party.Game.Camera;

public sealed class CameraServiceMobile : CameraService.Impl
{
    private readonly MediaPipeCameraHelper camera = MediaPipeCameraHelper.Instantiate();

    public CameraServiceMobile(CameraService owner)
    {
        owner.AddFeed(new CameraFeedMobile(camera, MediaPipeCameraHelper.CameraFacing.FacingFront));
        owner.AddFeed(new CameraFeedMobile(camera, MediaPipeCameraHelper.CameraFacing.FacingBack));
    }

    public override bool PermissionGranted()
    {
        return camera.PermissionGranted();
    }

    public override async Task<bool> RequestPermission()
    {
        if (PermissionGranted())
        {
            return true;
        }
        else
        {
            camera.RequestPermission();
            await camera.ToSignal(camera, "permission_request");
            return PermissionGranted();
        }
    }

    public override void Dispose()
    {
    }

    private sealed class CameraFeedMobile : CameraFeed
    {
        public override string Name => facing.ToString();
        public override int Width => DisplayServer.WindowGetSize().X;
        public override int Height => DisplayServer.WindowGetSize().Y;
        protected override bool Accelerated => true;

        private bool hasStarted;
        private readonly MediaPipeCameraHelper camera;
        private readonly MediaPipeCameraHelper.CameraFacing facing;

        public CameraFeedMobile(MediaPipeCameraHelper camera, MediaPipeCameraHelper.CameraFacing facing)
        {
            this.facing = facing;
            this.camera = camera;
        }

        public override void Start()
        {
            if (hasStarted)
            {
                return;
            }

            camera.SetGpuResources(MediaPipeGPUResources.Instantiate());
            camera.Start((int)facing, new Vector2(Width, Height));
            camera.NewFrame += onNewFrame;

            EmitStart();

            hasStarted = true;
        }

        public override void Close()
        {
            if (!hasStarted)
            {
                return;
            }

            EmitClose();

            camera.NewFrame -= onNewFrame;
            camera.Close();

            hasStarted = false;
        }

        private void onNewFrame(MediaPipeImage image)
        {
            if (image.IsGpuImage())
            {
                image.ConvertToCpu();
            }

            EmitFrame(image.GetImage());
        }
    }
}