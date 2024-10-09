using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeNormalizedKeypoint : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeNormalizedKeypoint";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeNormalizedKeypoint() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeNormalizedKeypoint"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeNormalizedKeypoint Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeNormalizedKeypoint>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeNormalizedKeypoint"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeNormalizedKeypoint"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeNormalizedKeypoint"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeNormalizedKeypoint"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeNormalizedKeypoint Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeNormalizedKeypoint>(godotObject);
    }
#region Properties

    public Vector2 Point
    {
        get => (Vector2)Get("point");
        set => Set("point", Variant.From(value));
    }

    public string Label
    {
        get => (string)Get("label");
        set => Set("label", Variant.From(value));
    }

    public float Score
    {
        get => (float)Get("score");
        set => Set("score", Variant.From(value));
    }

#endregion

#region Methods

    public bool HasLabel() => Call("has_label").As<bool>();

    public bool HasScore() => Call("has_score").As<bool>();

#endregion

}