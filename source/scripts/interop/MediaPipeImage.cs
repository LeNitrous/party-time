using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeImage : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeImage";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeImage() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeImage"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeImage Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeImage>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeImage"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeImage"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeImage"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeImage"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeImage Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeImage>(godotObject);
    }
#region Methods

    public static MediaPipeImage CreateFromImage(Image image) => GDExtensionHelper.Bind<MediaPipeImage>(GDExtensionHelper.Call("MediaPipeImage", "create_from_image", image).As<GodotObject>());

    public static MediaPipeImage CreateFromPacket(MediaPipePacket packet) => GDExtensionHelper.Bind<MediaPipeImage>(GDExtensionHelper.Call("MediaPipeImage", "create_from_packet", (RefCounted)packet).As<GodotObject>());

    public bool IsGpuImage() => Call("is_gpu_image").As<bool>();

    public void ConvertToCpu() => Call("convert_to_cpu");

    public Image GetImage() => Call("get_image").As<Image>();

    public void SetImage(Image image) => Call("set_image", image);

    public MediaPipePacket GetPacket() => GDExtensionHelper.Bind<MediaPipePacket>(Call("get_packet").As<GodotObject>());

    public MediaPipePacket GetImageFramePacket() => GDExtensionHelper.Bind<MediaPipePacket>(Call("get_image_frame_packet").As<GodotObject>());

    public void SetImageFromPacket(MediaPipePacket packet) => Call("set_image_from_packet", (RefCounted)packet);

#endregion

}