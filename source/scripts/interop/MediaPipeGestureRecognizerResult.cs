using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeGestureRecognizerResult : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeGestureRecognizerResult";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeGestureRecognizerResult() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeGestureRecognizerResult"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeGestureRecognizerResult Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeGestureRecognizerResult>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeGestureRecognizerResult"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeGestureRecognizerResult"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeGestureRecognizerResult"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeGestureRecognizerResult"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeGestureRecognizerResult Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeGestureRecognizerResult>(godotObject);
    }
#region Properties

    public Godot.Collections.Array<MediaPipeProto> Gestures
    {
        get => GDExtensionHelper.Cast<MediaPipeProto>((Godot.Collections.Array<Godot.GodotObject>)Get("gestures"));
        set => Set("gestures", Variant.From(value));
    }

    public Godot.Collections.Array<MediaPipeProto> Handedness
    {
        get => GDExtensionHelper.Cast<MediaPipeProto>((Godot.Collections.Array<Godot.GodotObject>)Get("handedness"));
        set => Set("handedness", Variant.From(value));
    }

    public Godot.Collections.Array<MediaPipeProto> HandLandmarks
    {
        get => GDExtensionHelper.Cast<MediaPipeProto>((Godot.Collections.Array<Godot.GodotObject>)Get("hand_landmarks"));
        set => Set("hand_landmarks", Variant.From(value));
    }

    public Godot.Collections.Array<MediaPipeProto> HandWorldLandmarks
    {
        get => GDExtensionHelper.Cast<MediaPipeProto>((Godot.Collections.Array<Godot.GodotObject>)Get("hand_world_landmarks"));
        set => Set("hand_world_landmarks", Variant.From(value));
    }

#endregion

}