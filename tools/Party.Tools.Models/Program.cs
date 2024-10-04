using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;

var model = File.Open(Path.Combine(AppContext.BaseDirectory, "lightweight-human-pose-estimation.onnx.zip"), FileMode.Open);
var image = File.Open(Path.Combine(AppContext.BaseDirectory, "input.jpg"), FileMode.Open);

var stopwatch = new Stopwatch();
var context = new MLContext();
var predict = context.Model.CreatePredictionEngine<Input, Output>(context.Model.Load(model, out _));

stopwatch.Start();
var prediction = predict.Predict(new Input(image));
Console.WriteLine("Prediction took {0}ms", stopwatch.ElapsedMilliseconds);
stopwatch.Stop();
Console.ReadKey();

// var context = new MLContext();

// var pipeline = context.Transforms
//     .ExtractPixels
//     (
//         inputColumnName: "input",
//         outputColumnName: "input.1",
//         outputAsFloatArray: true
//     )
//     .Append
//     (
//         context.Transforms.ApplyOnnxModel
//         (
//             modelFile: Path.Combine(AppContext.BaseDirectory, "lightweight-human-pose-estimation.onnx"),
//             inputColumnNames: ["input.1"],
//             outputColumnNames: ["345", "348", "397", "400"]
//         )
//     );

// var transformer = pipeline.Fit(context.Data.LoadFromEnumerable(new List<Input>()));

// using var output = File.Open(Path.Combine(AppContext.BaseDirectory, "lightweight-human-pose-estimation.onnx.zip"), FileMode.OpenOrCreate);
// context.Model.Save(transformer, null, output);

class Input
{
    [ColumnName("input")]
    [ImageType(320, 240)]
    public MLImage Image { get; set; }

    public Input(MLImage image)
    {
        Image = image;
    }

    public Input(Stream stream)
        : this(MLImage.CreateFromStream(stream))
    {
    }

    public Input(ReadOnlySpan<byte> bytes)
        : this(MLImage.CreateFromPixels(240, 320, MLPixelFormat.Rgba32, bytes))
    {
    }
}

class Output
{
    [ColumnName("345")]
    [VectorType([1, 19, 30, 40])]
    public float[] _345 { get; set; } = [];

    [ColumnName("348")]
    [VectorType([1, 38, 30, 40])]
    public float[] _348 { get; set; } = [];

    [ColumnName("397")]
    [VectorType([1, 19, 30, 40])]
    public float[] _397 { get; set; } = [];

    [ColumnName("400")]
    [VectorType([1, 38, 30, 40])]
    public float[] _400 { get; set; } = [];
}
