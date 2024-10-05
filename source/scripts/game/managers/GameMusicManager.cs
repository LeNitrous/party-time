using System.IO;
using System.Linq;
using Godot;
using NathanHoad;

namespace Party.Game.Experience.Managers;

public sealed partial class GameMusicManager : Node
{
    private Completion completion;

    public override void _Notification(int what)
    {
        if (what == NotificationPaused)
        {
            handleScenePaused(true);
        }

        if (what == NotificationUnpaused)
        {
            handleScenePaused(false);
        }
    }

    private void handleScenePaused(bool isPaused)
    {
        var stream = SoundManager.GetCurrentlyPlayingMusic()[0];

        if (loop.Contains(Path.GetFileName(stream.ResourcePath)))
        {
            return;
        }

        if (isPaused)
        {
            SoundManager.PauseMusic(stream);
        }
        else
        {
            SoundManager.ResumeMusic(stream);
        }
    }

    private void onCompletionChanged(Completion completion)
    {
        if (this.completion == completion)
        {
            return;
        }

        this.completion = completion;
    }

    private void onPhaseChanged(Phase phase)
    {
        if (phase == Phase.Starting)
        {
            completion = Completion.None;
        }

        string name = phase switch
        {
            Phase.Prologue => prologue,
            Phase.Epilogue => epilogue,
            Phase.Starting => starting.GetRandom(),
            Phase.Ending when completion is Completion.Win or Completion.WinTimeout => win.GetRandom(),
            Phase.Ending => lose.GetRandom(),
            Phase.InProgress => loop.GetRandom(),
            Phase.Transition => transition.GetRandom(),
            _ => string.Empty,
        };

        if (!string.IsNullOrEmpty(name))
        {
            string path = Path.Combine("res://", "sounds", "music", name);
            SoundManager.PlayMusic(GD.Load<AudioStream>(path), 0, "Music").PitchScale = (float)Engine.TimeScale;
        }
    }

    private static readonly string prologue = "game_prologue.mp3";
    private static readonly string epilogue = "game_epilogue.mp3";

    private static readonly string[] starting = new string[]
    {
        "game_new_1.mp3",
        "game_new_2.mp3",
        "game_new_3.mp3",
        "game_new_4.mp3",
        "game_new_5.mp3",
    };

    private static readonly string[] loop = new string[]
    {
        "game_loop_1.wav",
        "game_loop_2.wav",
    };

    private static readonly string[] win = new string[]
    {
        "game_ending_win_1.mp3",
        "game_ending_win_2.mp3",
        "game_ending_win_3.mp3",
    };

    private static readonly string[] lose = new string[]
    {
        "game_ending_lose_1.mp3",
        "game_ending_lose_2.mp3",
        "game_ending_lose_3.mp3",
    };

    private static readonly string[] transition = new string[]
    {
        "game_transition_1.mp3",
        "game_transition_2.mp3",
    };
}
