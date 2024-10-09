using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeTask : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeTask";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeTask() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeTask"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeTask Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeTask>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeTask"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeTask"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeTask"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeTask"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeTask Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeTask>(godotObject);
    }
#region Enums

    public enum AudioRunningMode : long
    {
        RunningModeAudioClips = 1,
        RunningModeAudioStream = 2,
    }

    public enum VisionRunningMode : long
    {
        RunningModeImage = 1,
        RunnineModeVideo = 2,
        RunningModeLiveStream = 3,
    }

#endregion

#region Methods

    public bool Initialize(MediaPipeTaskBaseOptions baseOptions) => Call("initialize", (RefCounted)baseOptions).As<bool>();

#endregion

}