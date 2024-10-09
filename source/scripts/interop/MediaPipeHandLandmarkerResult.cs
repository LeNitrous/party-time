using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeHandLandmarkerResult : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeHandLandmarkerResult";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeHandLandmarkerResult() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeHandLandmarkerResult"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeHandLandmarkerResult Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeHandLandmarkerResult>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeHandLandmarkerResult"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeHandLandmarkerResult"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeHandLandmarkerResult"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeHandLandmarkerResult"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeHandLandmarkerResult Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeHandLandmarkerResult>(godotObject);
    }
#region Properties

    public Godot.Collections.Array<MediaPipeClassifications> Handedness
    {
        get => GDExtensionHelper.Cast<MediaPipeClassifications>((Godot.Collections.Array<Godot.GodotObject>)Get("handedness"));
        set => Set("handedness", Variant.From(value));
    }

    public Godot.Collections.Array<MediaPipeNormalizedLandmarks> HandLandmarks
    {
        get => GDExtensionHelper.Cast<MediaPipeNormalizedLandmarks>((Godot.Collections.Array<Godot.GodotObject>)Get("hand_landmarks"));
        set => Set("hand_landmarks", Variant.From(value));
    }

    public Godot.Collections.Array<MediaPipeLandmarks> HandWorldLandmarks
    {
        get => GDExtensionHelper.Cast<MediaPipeLandmarks>((Godot.Collections.Array<Godot.GodotObject>)Get("hand_world_landmarks"));
        set => Set("hand_world_landmarks", Variant.From(value));
    }

#endregion

}