using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeClassificationResult : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeClassificationResult";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeClassificationResult() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeClassificationResult"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeClassificationResult Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeClassificationResult>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeClassificationResult"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeClassificationResult"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeClassificationResult"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeClassificationResult"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeClassificationResult Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeClassificationResult>(godotObject);
    }
#region Properties

    public Godot.Collections.Array<MediaPipeClassifications> Classifications
    {
        get => GDExtensionHelper.Cast<MediaPipeClassifications>((Godot.Collections.Array<Godot.GodotObject>)Get("classifications"));
        set => Set("classifications", Variant.From(value));
    }

    public int TimestampMs
    {
        get => (int)Get("timestamp_ms");
        set => Set("timestamp_ms", Variant.From(value));
    }

#endregion

#region Methods

    public bool HasTimestampMs() => Call("has_timestamp_ms").As<bool>();

#endregion

}