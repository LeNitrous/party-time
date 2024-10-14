using System.Linq;
using Godot;
using Party.Game.Detection;

namespace Party.Game.Experience.Events;

public abstract partial class GameEventMovementCheck : GameEventPoseLandmarker
{
    private Vector3[] current;

    protected virtual float Threshold => 0.015f;
    protected virtual int RequiredLandmarksMoved => 10;

    protected virtual bool ShouldDetect()
    {
        return true;
    }

    protected sealed override void OnDetect(PoseLandmarkerResult output)
    {
        if (!ShouldDetect())
        {
            return;
        }

        if (current is null)
        {
            current = output.ToArray();
            return;
        }

        int landmarksMoved = 0;

        for (int i = 0; i < (int)PoseLandmark.LeftKnee; i++)
        {
            var min = current[i] - (Vector3.One * Threshold);
            var max = current[i] + (Vector3.One * Threshold);

            var landmark = output[(PoseLandmark)i];

            if (landmark > max || landmark < min)
            {
                landmarksMoved++;
            }

            current[i] = landmark;
        }

        if (landmarksMoved >= RequiredLandmarksMoved)
        {
            OnMovement();
        }
    }

    protected abstract void OnMovement();
}
