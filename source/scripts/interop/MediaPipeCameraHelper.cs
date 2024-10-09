using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeCameraHelper : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeCameraHelper";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeCameraHelper() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeCameraHelper"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeCameraHelper Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeCameraHelper>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeCameraHelper"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeCameraHelper"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeCameraHelper"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeCameraHelper"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeCameraHelper Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeCameraHelper>(godotObject);
    }
#region Enums

    public enum CameraFacing : long
    {
        FacingFront = 0,
        FacingBack = 1,
    }

#endregion

#region Signals

    public delegate void PermissionResultHandler(bool granted);

    private PermissionResultHandler _permissionResult_backing;
    private Callable _permissionResult_backing_callable;
    public event PermissionResultHandler PermissionResult
    {
        add
        {
            if(_permissionResult_backing == null)
            {
                _permissionResult_backing_callable = Callable.From<Variant>(
                    (arg0_variant) =>
                    {
                        var arg0 = arg0_variant.As<bool>();
                        _permissionResult_backing?.Invoke(arg0);
                    }
                );
                Connect("permission_result", _permissionResult_backing_callable);
            }
            _permissionResult_backing += value;
        }
        remove
        {
            _permissionResult_backing -= value;
            
            if(_permissionResult_backing == null)
            {
                Disconnect("permission_result", _permissionResult_backing_callable);
                _permissionResult_backing_callable = default;
            }
        }
    }

    public delegate void NewFrameHandler(MediaPipeImage image);

    private NewFrameHandler _newFrame_backing;
    private Callable _newFrame_backing_callable;
    public event NewFrameHandler NewFrame
    {
        add
        {
            if(_newFrame_backing == null)
            {
                _newFrame_backing_callable = Callable.From<Variant>(
                    (arg0_variant) =>
                    {
                        var arg0 = GDExtensionHelper.Bind<MediaPipeImage>(arg0_variant.As<GodotObject>());
                        _newFrame_backing?.Invoke(arg0);
                    }
                );
                Connect("new_frame", _newFrame_backing_callable);
            }
            _newFrame_backing += value;
        }
        remove
        {
            _newFrame_backing -= value;
            
            if(_newFrame_backing == null)
            {
                Disconnect("new_frame", _newFrame_backing_callable);
                _newFrame_backing_callable = default;
            }
        }
    }

#endregion

#region Methods

    public bool PermissionGranted() => Call("permission_granted").As<bool>();

    public void RequestPermission() => Call("request_permission");

    public void SetMirrored(bool value) => Call("set_mirrored", value);

    public void SetGpuResources(MediaPipeGPUResources gpuResources) => Call("set_gpu_resources", (RefCounted)gpuResources);

    public void Start(int index, Vector2 size) => Call("start", index, size);

    public void Close() => Call("close");

#endregion

}