using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipePacket : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipePacket";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipePacket() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipePacket"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipePacket Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipePacket>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipePacket"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipePacket"/> wrapper type,
    /// a new instance of the <see cref="MediaPipePacket"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipePacket"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipePacket Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipePacket>(godotObject);
    }
#region Properties

    public int Timestamp
    {
        get => (int)Get("timestamp");
        set => Set("timestamp", Variant.From(value));
    }

#endregion

#region Methods

    public bool IsEmpty() => Call("is_empty").As<bool>();

    public void Get() => Call("get");

    public bool Set(Variant? value) => Call("set", value ?? new Variant()).As<bool>();

    public MediaPipeProto GetProto(string typeName) => GDExtensionHelper.Bind<MediaPipeProto>(Call("get_proto", typeName).As<GodotObject>());

    public Godot.Collections.Array<MediaPipeProto> GetProtoVector(string typeName) => GDExtensionHelper.Cast<MediaPipeProto>(Call("get_proto_vector", typeName).As<Godot.Collections.Array<Godot.GodotObject>>());

#endregion

}