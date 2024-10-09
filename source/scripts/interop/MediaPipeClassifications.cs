using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeClassifications : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeClassifications";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeClassifications() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeClassifications"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeClassifications Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeClassifications>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeClassifications"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeClassifications"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeClassifications"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeClassifications"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeClassifications Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeClassifications>(godotObject);
    }
#region Properties

    public Godot.Collections.Array<MediaPipeCategory> Categories
    {
        get => GDExtensionHelper.Cast<MediaPipeCategory>((Godot.Collections.Array<Godot.GodotObject>)Get("categories"));
        set => Set("categories", Variant.From(value));
    }

    public int HeadIndex
    {
        get => (int)Get("head_index");
        set => Set("head_index", Variant.From(value));
    }

    public string HeadName
    {
        get => (string)Get("head_name");
        set => Set("head_name", Variant.From(value));
    }

#endregion

#region Methods

    public bool HasHeadName() => Call("has_head_name").As<bool>();

#endregion

}