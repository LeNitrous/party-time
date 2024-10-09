using System;
using Godot;

namespace GDExtension.Wrappers;

public abstract partial class AudioEffectFilterInstance : AudioEffectInstance
{
    public static readonly StringName GDExtensionName = "AudioEffectFilterInstance";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying AudioEffectInstance), please use the Instantiate() method instead.")]
    protected AudioEffectFilterInstance() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="AudioEffectFilterInstance"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static AudioEffectFilterInstance Instantiate()
    {
        return GDExtensionHelper.Instantiate<AudioEffectFilterInstance>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="AudioEffectFilterInstance"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="AudioEffectFilterInstance"/> wrapper type,
    /// a new instance of the <see cref="AudioEffectFilterInstance"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="AudioEffectFilterInstance"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static AudioEffectFilterInstance Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<AudioEffectFilterInstance>(godotObject);
    }
}