using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipePoseLandmarkerResult : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipePoseLandmarkerResult";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipePoseLandmarkerResult() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipePoseLandmarkerResult"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipePoseLandmarkerResult Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipePoseLandmarkerResult>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipePoseLandmarkerResult"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipePoseLandmarkerResult"/> wrapper type,
    /// a new instance of the <see cref="MediaPipePoseLandmarkerResult"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipePoseLandmarkerResult"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipePoseLandmarkerResult Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipePoseLandmarkerResult>(godotObject);
    }
#region Properties

    public Godot.Collections.Array<MediaPipeImage> SegmentationMasks
    {
        get => GDExtensionHelper.Cast<MediaPipeImage>((Godot.Collections.Array<Godot.GodotObject>)Get("segmentation_masks"));
        set => Set("segmentation_masks", Variant.From(value));
    }

    public Godot.Collections.Array<MediaPipeNormalizedLandmarks> PoseLandmarks
    {
        get => GDExtensionHelper.Cast<MediaPipeNormalizedLandmarks>((Godot.Collections.Array<Godot.GodotObject>)Get("pose_landmarks"));
        set => Set("pose_landmarks", Variant.From(value));
    }

    public Godot.Collections.Array<MediaPipeLandmarks> PoseWorldLandmarks
    {
        get => GDExtensionHelper.Cast<MediaPipeLandmarks>((Godot.Collections.Array<Godot.GodotObject>)Get("pose_world_landmarks"));
        set => Set("pose_world_landmarks", Variant.From(value));
    }

#endregion

#region Methods

    public bool HasSegmentationMasks() => Call("has_segmentation_masks").As<bool>();

#endregion

}