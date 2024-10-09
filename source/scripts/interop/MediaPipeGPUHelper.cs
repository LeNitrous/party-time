using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeGPUHelper : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeGPUHelper";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeGPUHelper() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeGPUHelper"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeGPUHelper Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeGPUHelper>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeGPUHelper"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeGPUHelper"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeGPUHelper"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeGPUHelper"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeGPUHelper Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeGPUHelper>(godotObject);
    }
#region Methods

    public void Initialize(MediaPipeGPUResources gpuResources) => Call("initialize", (RefCounted)gpuResources);

    public MediaPipeImage MakeGpuImage(MediaPipeImage image) => GDExtensionHelper.Bind<MediaPipeImage>(Call("make_gpu_image", (RefCounted)image).As<GodotObject>());

    public MediaPipePacket MakeGpuBufferPacket(MediaPipeImage image) => GDExtensionHelper.Bind<MediaPipePacket>(Call("make_gpu_buffer_packet", (RefCounted)image).As<GodotObject>());

#endregion

}