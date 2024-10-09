using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeGraphConfig : Resource
{
    public static readonly StringName GDExtensionName = "MediaPipeGraphConfig";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying Resource), please use the Instantiate() method instead.")]
    protected MediaPipeGraphConfig() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeGraphConfig"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeGraphConfig Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeGraphConfig>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeGraphConfig"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeGraphConfig"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeGraphConfig"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeGraphConfig"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeGraphConfig Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeGraphConfig>(godotObject);
    }
#region Methods

    public bool HasInputStream(string streamName) => Call("has_input_stream", streamName).As<bool>();

    public bool HasOutputStream(string streamName) => Call("has_output_stream", streamName).As<bool>();

    public int Load(string path, bool asBinary) => Call("load", path, asBinary).As<int>();

    public bool ParseBytes(byte[] data) => Call("parse_bytes", data).As<bool>();

    public bool ParseText(string data) => Call("parse_text", data).As<bool>();

#endregion

}