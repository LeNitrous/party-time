using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeImageSegmenterResult : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeImageSegmenterResult";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeImageSegmenterResult() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeImageSegmenterResult"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeImageSegmenterResult Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeImageSegmenterResult>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeImageSegmenterResult"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeImageSegmenterResult"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeImageSegmenterResult"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeImageSegmenterResult"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeImageSegmenterResult Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeImageSegmenterResult>(godotObject);
    }
#region Properties

    public Godot.Collections.Array<MediaPipeImage> ConfidenceMasks
    {
        get => GDExtensionHelper.Cast<MediaPipeImage>((Godot.Collections.Array<Godot.GodotObject>)Get("confidence_masks"));
        set => Set("confidence_masks", Variant.From(value));
    }

    public MediaPipeImage CategoryMask
    {
        get => (MediaPipeImage)Get("category_mask");
        set => Set("category_mask", Variant.From(value));
    }

#endregion

#region Methods

    public bool HasConfidenceMasks() => Call("has_confidence_masks").As<bool>();

    public bool HasCategoryMask() => Call("has_category_mask").As<bool>();

#endregion

}