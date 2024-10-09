using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeDetectionResult : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeDetectionResult";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeDetectionResult() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeDetectionResult"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeDetectionResult Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeDetectionResult>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeDetectionResult"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeDetectionResult"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeDetectionResult"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeDetectionResult"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeDetectionResult Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeDetectionResult>(godotObject);
    }
#region Properties

    public Godot.Collections.Array<MediaPipeDetection> Detections
    {
        get => GDExtensionHelper.Cast<MediaPipeDetection>((Godot.Collections.Array<Godot.GodotObject>)Get("detections"));
        set => Set("detections", Variant.From(value));
    }

#endregion

}