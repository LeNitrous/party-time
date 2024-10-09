using System;
using Godot;

namespace GDExtension.Wrappers;

public partial class MediaPipeImageSegmenter : MediaPipeTask
{
    public new static readonly StringName GDExtensionName = "MediaPipeImageSegmenter";

    [Obsolete("Wrapper classes cannot be constructed with Ctor (it only instantiate the underlying RefCounted), please use the Instantiate() method instead.")]
    protected MediaPipeImageSegmenter() { }

    /// <summary>
    /// Creates an instance of the GDExtension <see cref="MediaPipeImageSegmenter"/> type, and attaches the wrapper script to it.
    /// </summary>
    /// <returns>The wrapper instance linked to the underlying GDExtension type.</returns>
    public new static MediaPipeImageSegmenter Instantiate()
    {
        return GDExtensionHelper.Instantiate<MediaPipeImageSegmenter>(GDExtensionName);
    }

    /// <summary>
    /// Try to cast the script on the supplied <paramref name="godotObject"/> to the <see cref="MediaPipeImageSegmenter"/> wrapper type,
    /// if no script has attached to the type, or the script attached to the type does not inherit the <see cref="MediaPipeImageSegmenter"/> wrapper type,
    /// a new instance of the <see cref="MediaPipeImageSegmenter"/> wrapper script will get attaches to the <paramref name="godotObject"/>.
    /// </summary>
    /// <remarks>The developer should only supply the <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</remarks>
    /// <param name="godotObject">The <paramref name="godotObject"/> that represents the correct underlying GDExtension type.</param>
    /// <returns>The existing or a new instance of the <see cref="MediaPipeImageSegmenter"/> wrapper script attached to the supplied <paramref name="godotObject"/>.</returns>
    public new static MediaPipeImageSegmenter Bind(GodotObject godotObject)
    {
        return GDExtensionHelper.Bind<MediaPipeImageSegmenter>(godotObject);
    }
#region Signals

    public delegate void ResultCallbackHandler(MediaPipeImageSegmenterResult result, MediaPipeImage image, int timestampMs);

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
                        var arg0 = GDExtensionHelper.Bind<MediaPipeImageSegmenterResult>(arg0_variant.As<GodotObject>());
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

    public bool Initialize(MediaPipeTaskBaseOptions baseOptions, int runningMode, string displayNamesLocale, bool outputConfidenceMasks, bool outputCategoryMask) => Call("initialize", (RefCounted)baseOptions, runningMode, displayNamesLocale, outputConfidenceMasks, outputCategoryMask).As<bool>();

    public MediaPipeImageSegmenterResult Segment(MediaPipeImage image, Rect2 regionOfInterest, int rotationDegrees) => GDExtensionHelper.Bind<MediaPipeImageSegmenterResult>(Call("segment", (RefCounted)image, regionOfInterest, rotationDegrees).As<GodotObject>());

    public MediaPipeImageSegmenterResult SegmentVideo(MediaPipeImage image, int regionOfInterest, Rect2 rotationDegrees, int unnamedArg3) => GDExtensionHelper.Bind<MediaPipeImageSegmenterResult>(Call("segment_video", (RefCounted)image, regionOfInterest, rotationDegrees, unnamedArg3).As<GodotObject>());

    public bool SegmentAsync(MediaPipeImage image, int regionOfInterest, Rect2 rotationDegrees, int unnamedArg3) => Call("segment_async", (RefCounted)image, regionOfInterest, rotationDegrees, unnamedArg3).As<bool>();

    public string[] GetLabels() => Call("get_labels").As<string[]>();

#endregion

}