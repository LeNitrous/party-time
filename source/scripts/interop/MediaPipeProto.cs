using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeProto : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeProto";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeProto() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeProto"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeProto Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeProto>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeProto"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeProto"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeProto"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeProto"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeProto Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeProto>(godotObject);
    }
#region Methods

    public bool Initialize(string typeName) => Call("initialize", typeName).As<bool>();

    public bool IsInitialized() => Call("is_initialized").As<bool>();

    public string GetProtoType() => Call("get_type").As<string>();

    public string[] GetFields() => Call("get_fields").As<string[]>();

    public bool IsRepeatedField(string fieldName) => Call("is_repeated_field", fieldName).As<bool>();

    public int GetFieldSize(string fieldName) => Call("get_field_size", fieldName).As<int>();

    public void Get(string fieldName) => Call("get", fieldName);

    public void GetRepeated(string fieldName, int index) => Call("get_repeated", fieldName, index);

    public bool Set(string fieldName, Variant? value) => Call("set", fieldName, value ?? new Variant()).As<bool>();

#endregion

}