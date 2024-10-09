using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeDetection : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeDetection";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeDetection() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeDetection"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeDetection Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeDetection>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeDetection"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeDetection"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeDetection"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeDetection"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeDetection Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeDetection>(godotObject);
    }
#region Properties

    public Godot.Collections.Array<MediaPipeCategory> Categories
    {
        get => GDExtensionHelper.Cast<MediaPipeCategory>((Godot.Collections.Array<Godot.GodotObject>)Get("categories"));
        set => Set("categories", Variant.From(value));
    }

    public Rect2I BoundingBox
    {
        get => (Rect2I)Get("bounding_box");
        set => Set("bounding_box", Variant.From(value));
    }

    public Godot.Collections.Array<MediaPipeNormalizedKeypoint> Keypoints
    {
        get => GDExtensionHelper.Cast<MediaPipeNormalizedKeypoint>((Godot.Collections.Array<Godot.GodotObject>)Get("keypoints"));
        set => Set("keypoints", Variant.From(value));
    }

#endregion

#region Methods

    public bool HasKeypoints() => Call("has_keypoints").As<bool>();

#endregion

}