using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeTaskBaseOptions : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeTaskBaseOptions";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeTaskBaseOptions() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeTaskBaseOptions"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeTaskBaseOptions Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeTaskBaseOptions>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeTaskBaseOptions"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeTaskBaseOptions"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeTaskBaseOptions"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeTaskBaseOptions"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeTaskBaseOptions Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeTaskBaseOptions>(godotObject);
    }
#region Enums

    public enum DelegateEnum : long
    {
        DelegateCpu = 0,
        DelegateGpu = 1,
        DelegateEdgetpuNnapi = 2,
    }

#endregion

#region Properties

    public byte[] ModelAssetBuffer
    {
        get => (byte[])Get("model_asset_buffer");
        set => Set("model_asset_buffer", Variant.From(value));
    }

    public string ModelAssetPath
    {
        get => (string)Get("model_asset_path");
        set => Set("model_asset_path", Variant.From(value));
    }

    public int Delegate
    {
        get => (int)Get("delegate");
        set => Set("delegate", Variant.From(value));
    }

#endregion

}