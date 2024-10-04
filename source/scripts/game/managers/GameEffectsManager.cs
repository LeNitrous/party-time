using System.IO;
using Godot;
using NathanHoad;

namespace Party.Game.Experience.Managers;

public sealed partial class GameEffectsManager : Node
{
    private Completion completion;

    private void onCompletionChanged(Completion completion)
    {
        if (this.completion == completion)
        {
            return;
        }

        if (completion is Completion.Win or Completion.Lose)
        {
            string name = completion is Completion.Win ? win.GetRandom() : lose.GetRandom();
            string path = Path.Combine("res://", "sounds", "effects", name);
            SoundManager.PlaySoundWithPitch(GD.Load<AudioStream>(path), (float)Engine.TimeScale, "Effects");
        }

        this.completion = completion;
    }

    private void onPhaseChanged(Phase phase)
    {
        if (phase == Phase.Starting)
        {
            completion = Completion.None;
        }
    }

    private static readonly string[] win = new string[]
    {
        "game_won_1.wav",
        "game_won_2.wav",
        "game_won_3.wav",
    };

    private static readonly string[] lose = new string[]
    {
        "game_lose_1.wav",
        "game_lose_2.wav",
        "game_lose_3.wav",
    };
}
