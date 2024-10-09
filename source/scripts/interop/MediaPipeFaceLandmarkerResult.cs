using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeFaceLandmarkerResult : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeFaceLandmarkerResult";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeFaceLandmarkerResult() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeFaceLandmarkerResult"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeFaceLandmarkerResult Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeFaceLandmarkerResult>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeFaceLandmarkerResult"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeFaceLandmarkerResult"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeFaceLandmarkerResult"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeFaceLandmarkerResult"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeFaceLandmarkerResult Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeFaceLandmarkerResult>(godotObject);
    }
#region Properties

    public Godot.Collections.Array<MediaPipeNormalizedLandmarks> FaceLandmarks
    {
        get => GDExtensionHelper.Cast<MediaPipeNormalizedLandmarks>((Godot.Collections.Array<Godot.GodotObject>)Get("face_landmarks"));
        set => Set("face_landmarks", Variant.From(value));
    }

    public Godot.Collections.Array<MediaPipeClassifications> FaceBlendshapes
    {
        get => GDExtensionHelper.Cast<MediaPipeClassifications>((Godot.Collections.Array<Godot.GodotObject>)Get("face_blendshapes"));
        set => Set("face_blendshapes", Variant.From(value));
    }

    public Godot.Collections.Array FacialTransformationMatrixes
    {
        get => (Godot.Collections.Array)Get("facial_transformation_matrixes");
        set => Set("facial_transformation_matrixes", Variant.From(value));
    }

#endregion

#region Methods

    public bool HasFaceBlendshapes() => Call("has_face_blendshapes").As<bool>();

    public bool HasFacialTransformationMatrixes() => Call("has_facial_transformation_matrixes").As<bool>();

#endregion

}