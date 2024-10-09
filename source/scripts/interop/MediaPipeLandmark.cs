using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeLandmark : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeLandmark";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeLandmark() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeLandmark"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeLandmark Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeLandmark>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeLandmark"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeLandmark"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeLandmark"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeLandmark"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeLandmark Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeLandmark>(godotObject);
    }
#region Properties

    public float X
    {
        get => (float)Get("x");
        set => Set("x", Variant.From(value));
    }

    public float Y
    {
        get => (float)Get("y");
        set => Set("y", Variant.From(value));
    }

    public float Z
    {
        get => (float)Get("z");
        set => Set("z", Variant.From(value));
    }

    public float Visibility
    {
        get => (float)Get("visibility");
        set => Set("visibility", Variant.From(value));
    }

    public float Presence
    {
        get => (float)Get("presence");
        set => Set("presence", Variant.From(value));
    }

    public string Name
    {
        get => (string)Get("name");
        set => Set("name", Variant.From(value));
    }

#endregion

#region Methods

    public bool HasVisibility() => Call("has_visibility").As<bool>();

    public bool HasPresence() => Call("has_presence").As<bool>();

    public bool HasName() => Call("has_name").As<bool>();

#endregion

}