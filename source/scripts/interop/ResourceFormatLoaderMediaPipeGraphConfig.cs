using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class ResourceFormatLoaderMediaPipeGraphConfig : ResourceFormatLoader
{
    public static readonly StringName GDExtensionName = "ResourceFormatLoaderMediaPipeGraphConfig";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying ResourceFormatLoader), please use the Instantiate() method instead.")]
    protected ResourceFormatLoaderMediaPipeGraphConfig() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="ResourceFormatLoaderMediaPipeGraphConfig"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static ResourceFormatLoaderMediaPipeGraphConfig Instantiate()
    {
        return GDExtensionHelper.Instantiate<ResourceFormatLoaderMediaPipeGraphConfig>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="ResourceFormatLoaderMediaPipeGraphConfig"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="ResourceFormatLoaderMediaPipeGraphConfig"/> wrapper type,
    /// a new instance of the <see cref="ResourceFormatLoaderMediaPipeGraphConfig"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="ResourceFormatLoaderMediaPipeGraphConfig"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static ResourceFormatLoaderMediaPipeGraphConfig Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<ResourceFormatLoaderMediaPipeGraphConfig>(godotObject);
    }
}