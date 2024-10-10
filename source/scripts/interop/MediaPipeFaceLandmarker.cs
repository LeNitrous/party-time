using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeFaceLandmarker : MediaPipeTask
{
    public new static readonly StringName GDExtensionName = "MediaPipeFaceLandmarker";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeFaceLandmarker() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeFaceLandmarker"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public new static MediaPipeFaceLandmarker Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeFaceLandmarker>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeFaceLandmarker"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeFaceLandmarker"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeFaceLandmarker"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeFaceLandmarker"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public new static MediaPipeFaceLandmarker Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeFaceLandmarker>(godotObject);
    }
#region Signals

    public delegate void ResultCallbackHandler(MediaPipeFaceLandmarkerResult result, MediaPipeImage image, int timestampMs);

    private ResultCallbackHandler _resultCallback_backing;
    private Callable _resultCallback_backing_callable;
    public event ResultCallbackHandler ResultCallback
    {
        add
        {
            if(_resultCallback_backing == null)
            {
                _resultCallback_backing_callable = Callable.From<Variant, Variant, Variant>(
                    (arg0_variant, arg1_variant, arg2_variant) =>
                    {
                        var arg0 = GDExtensionHelper.Bind<MediaPipeFaceLandmarkerResult>(arg0_variant.As<GodotObject>());
                        var arg1 = GDExtensionHelper.Bind<MediaPipeImage>(arg1_variant.As<GodotObject>());
                        var arg2 = arg2_variant.As<int>();
                        _resultCallback_backing?.Invoke(arg0, arg1, arg2);
                    }
                );
                Connect("result_callback", _resultCallback_backing_callable);
            }
            _resultCallback_backing += value;
        }
        remove
        {
            _resultCallback_backing -= value;
            
            if(_resultCallback_backing == null)
            {
                Disconnect("result_callback", _resultCallback_backing_callable);
                _resultCallback_backing_callable = default;
            }
        }
    }

#endregion

#region Methods

    public bool Initialize(MediaPipeTaskBaseOptions baseOptions, int runningMode, int numFaces, float minFaceDetectionConfidence, float minFacePresenceConfidence, float minTrackingConfidence, bool outputFaceBlendshapes, bool outputFacialTransformationMatrixes) => Call("initialize", (RefCounted)baseOptions, runningMode, numFaces, minFaceDetectionConfidence, minFacePresenceConfidence, minTrackingConfidence, outputFaceBlendshapes, outputFacialTransformationMatrixes).As<bool>();

    public MediaPipeFaceLandmarkerResult Detect(MediaPipeImage image, Rect2 regionOfInterest, int rotationDegrees) => GDExtensionHelper.Bind<MediaPipeFaceLandmarkerResult>(Call("detect", (RefCounted)image, regionOfInterest, rotationDegrees).As<GodotObject>());

    public MediaPipeFaceLandmarkerResult DetectVideo(MediaPipeImage image, int timestampMs, Rect2 regionOfInterest, int rotationDegrees) => GDExtensionHelper.Bind<MediaPipeFaceLandmarkerResult>(Call("detect_video", (RefCounted)image, timestampMs, regionOfInterest, rotationDegrees).As<GodotObject>());

    public bool DetectAsync(MediaPipeImage image, int timestampMs, Rect2 regionOfInterest, int rotationDegrees) => Call("detect_async", (RefCounted)image, timestampMs, regionOfInterest, rotationDegrees).As<bool>();

#endregion

}