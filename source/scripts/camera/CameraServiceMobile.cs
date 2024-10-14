using System.Threading;
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
            await camera.ToSignal(camera, "permission_result");
            return PermissionGranted();
        }
    }

    public override void Dispose()
    {
    }

    private sealed class CameraFeedMobile : CameraFeed
    {
        public override string Name { get; } = "Camera";
        public override int Width => 1280;
        public override int Height => 720;

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
            EmitFrame(image);
        }
    }
}