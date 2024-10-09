using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeCategory : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeCategory";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeCategory() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeCategory"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeCategory Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeCategory>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeCategory"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeCategory"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeCategory"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeCategory"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeCategory Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeCategory>(godotObject);
    }
#region Properties

    public int Index
    {
        get => (int)Get("index");
        set => Set("index", Variant.From(value));
    }

    public float Score
    {
        get => (float)Get("score");
        set => Set("score", Variant.From(value));
    }

    public string CategoryName
    {
        get => (string)Get("category_name");
        set => Set("category_name", Variant.From(value));
    }

    public string DisplayName
    {
        get => (string)Get("display_name");
        set => Set("display_name", Variant.From(value));
    }

#endregion

#region Methods

    public bool HasCategoryName() => Call("has_category_name").As<bool>();

    public bool HasDisplayName() => Call("has_display_name").As<bool>();

#endregion

}