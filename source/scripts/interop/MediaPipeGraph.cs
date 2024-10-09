using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeGraph : RefCounted
{
    public static readonly StringName GDExtensionName = "MediaPipeGraph";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeGraph() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeGraph"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public static MediaPipeGraph Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeGraph>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeGraph"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeGraph"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeGraph"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeGraph"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public static MediaPipeGraph Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeGraph>(godotObject);
    }
#region Methods

    public void Initialize(MediaPipeGraphConfig config) => Call("initialize", (Resource)config);

    public bool IsInitialized() => Call("is_initialized").As<bool>();

    public bool HasInputStream(string streamName) => Call("has_input_stream", streamName).As<bool>();

    public bool HasOutputStream(string streamName) => Call("has_output_stream", streamName).As<bool>();

    public void AddPacketCallback(string streamName, Callable callback) => Call("add_packet_callback", streamName, callback);

    public void SetSidePacket(string streamName, MediaPipePacket packet) => Call("set_side_packet", streamName, (RefCounted)packet);

    public void SetGpuResources(MediaPipeGPUResources gpuResources) => Call("set_gpu_resources", (RefCounted)gpuResources);

    public void Start() => Call("start");

    public void Stop() => Call("stop");

    public bool IsRunning() => Call("is_running").As<bool>();

    public void AddPacket(string streamName, MediaPipePacket packet) => Call("add_packet", streamName, (RefCounted)packet);

#endregion

}