using System;
using SkiaSharp;
using YoloDotNet;
using YoloDotNet.Enums;
using YoloDotNet.Models;

/// <summary>
/// A task that runs a YOLO model on a given image.
/// This class is not meant to be used directly, but rather through the <see cref="IJob"/>.
/// </summary>
public class YoloTask
{
	private readonly Yolo yolo;
	private readonly ModelType modelType;

	/// <summary>
	/// Initializes a new instance of the <see cref="YoloTask"/> class.
	/// </summary>
	/// <param name="modelPath"> The path to the specific model </param>
	/// <param name="modelType"> The model type to run in the task </param>
	/// <param name="shouldUseCuda"> Should the task use CUDA? Defaults to false. </param>
	/// <param name="gpuId"> The GPU ID to run inference to, defaults to GPU ID 0. </param>
	public YoloTask(string modelPath, ModelType modelType, bool shouldUseCuda = false, int gpuId = 0)
	{
		this.modelType = modelType;

		yolo = new Yolo(new YoloOptions()
		{
			OnnxModel = modelPath,
			ModelType = modelType,
			Cuda = shouldUseCuda,
			GpuId = gpuId
		});
	}

	/// <summary>
	/// Runs the YOLO model on the given image.
	/// </summary>
	/// <param name="image"> the image to run inference to </param>
	/// <param name="minConfidence"> the minimum confidence, decimal-percentage format. </param>
	/// <returns> depending on the modelType set in the ctor, it may return <see cref="PoseEstimation"/>, <see cref="ObjectDetection"/> or <see cref="Segmentation"/>. </returns>
	public dynamic OnFrameInput(SKImage image, float minConfidence = 0.8f)
	{
		return modelType switch
		{
			ModelType.PoseEstimation => yolo.RunPoseEstimation(image, minConfidence),
			ModelType.ObjectDetection => yolo.RunObjectDetection(image, minConfidence),
			ModelType.Segmentation => yolo.RunSegmentation(image, minConfidence),
			// this should never ever EVER hppen, but if it does, may god have mercy on your soul.
			_ => throw new NotImplementedException("The worker only supports PoseEstimation, ObjectDetection and Segmentation models."),
		};
	}
}
