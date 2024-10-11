using Godot;

namespace Party.Game.Detection;

public sealed partial class GestureRecognizerAnnotator : DetectionAnnotator<GestureRecognizerResult>
{
    protected override void Render(GestureRecognizerResult output)
    {
        if (output.Count <= 0)
        {
            return;
        }

        foreach (var hand in output)
        {
            var rect = new Rect2(hand.Bounds.Position * Size, hand.Bounds.Size * Size);
            DrawRect(rect, Colors.Green, false, 3.0f, false);
            DrawRect(new Rect2(rect.Position, new Vector2(rect.Size.X, -20)), Colors.Green);
            DrawString(ThemeDB.FallbackFont, rect.Position, $"Handedness: {hand.Handedness} / Gesture: {hand.Gesture}", modulate: Colors.Black);
        }
    }
}