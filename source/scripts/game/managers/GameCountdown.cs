using System;
using System.IO;
using Godot;
using NathanHoad;

namespace Party.Game.Experience.Managers;

public sealed partial class GameCountdown : Node
{
    public static Announcer Voice = Announcer.Random;

    private Timer time;
    private Timer wait;
    private string announcer;
    private bool isCountdownActive;

    public override void _Ready()
    {
        time = GetNode<Timer>("Time");
        wait = GetNode<Timer>("Wait");
        announcer = getAnnouncerFolder(Voice);
    }

    private void startCountdownVoice(int from)
    {
        if (isCountdownActive)
        {
            return;
        }

        if (from is > 10 or < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(from));
        }

        int index = from - 1;

        void doCountdownVoice()
        {
            if (index < 0)
            {
                time.Disconnect(Timer.SignalName.Timeout, Callable.From(doCountdownVoice));
                isCountdownActive = false;
                return;
            }

            speak(countdown[index--]);
        }

        time.Start(1.0);
        time.Connect(Timer.SignalName.Timeout, Callable.From(doCountdownVoice));

        doCountdownVoice();

        isCountdownActive = true;
    }

    private void onPhaseChanged(Phase phase)
    {
        void startCountdownOnFive()
        {
            startCountdownVoice(5);
        }

        if (phase == Phase.Prologue)
        {
            var tween = CreateTween();
            var label = GetNode<Label>("%Ready");

            tween
                .SetParallel(true);

            tween
                .TweenProperty(label, "scale", new Vector2(1, 1), 0.3)
                .From(new Vector2(0.5f, 0.5f))
                .SetDelay(GameContext.Duration.Prologue - 3.0)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Back);

            tween
                .TweenProperty(label, "modulate", Colors.White, 0.3)
                .From(Colors.Transparent)
                .SetDelay(GameContext.Duration.Prologue - 3.0);

            tween
                .TweenCallback(Callable.From(() => label.Text = "SET"))
                .SetDelay(GameContext.Duration.Prologue - 2.0);

            tween
                .TweenCallback(Callable.From(() => label.Text = "GO"))
                .SetDelay(GameContext.Duration.Prologue - 1.0);

            tween
                .TweenCallback(Callable.From(label.Hide))
                .SetDelay(GameContext.Duration.Prologue);

            tween
                .TweenCallback(Callable.From(() => speak("ready")))
                .SetDelay(GameContext.Duration.Prologue - 3.0);

            tween
                .TweenCallback(Callable.From(() => speak("set")))
                .SetDelay(GameContext.Duration.Prologue - 2.0);

            tween
                .TweenCallback(Callable.From(() => speak("go")))
                .SetDelay(GameContext.Duration.Prologue - 1.0);

            tween.Play();
        }

        if (phase == Phase.Starting)
        {
            var context = GetNode<Timer>("%Context/Time");

            if (context.TimeLeft > 0.0)
            {
                var callable = Callable.From(startCountdownOnFive);

                if (!wait.IsConnected(Timer.SignalName.Timeout, callable))
                {
                    wait.Connect(Timer.SignalName.Timeout, callable, (uint)ConnectFlags.OneShot);
                }

                wait.Start(context.WaitTime - 5.0);
            }
        }

        if (phase == Phase.Ending)
        {
            wait.Stop();
        }
    }

    private void onCompletionChanged(Completion completion)
    {
        if (completion is Completion.WinTimeout or Completion.LoseTimeout)
        {
            return;
        }

        wait.Stop();
    }

    private void speak(string line)
    {
        string path = Path.Combine("res://", "sounds", "voice", announcer, Path.ChangeExtension(line, ".ogg"));

        if (ResourceLoader.Exists(path))
        {
            SoundManager.PlaySound(GD.Load<AudioStream>(path), "Voice");
        }
        else
        {
            GD.PrintErr(nameof(GameCountdown), " :: tried to play \"", path, "\" but the file does not exist!");
        }
    }

    private static string getAnnouncerFolder(Announcer announcer)
    {
        return announcer switch
        {
            Announcer.Male => announcers[0],
            Announcer.Female => announcers[1],
            _ => announcers.GetRandom(),
        };
    }

    private static readonly string[] countdown =
    [
        "1.ogg",
        "2.ogg",
        "3.ogg",
        "4.ogg",
        "5.ogg",
        "6.ogg",
        "7.ogg",
        "8.ogg",
        "9.ogg",
        "10.ogg",
    ];
    private static readonly string[] announcers = ["a", "b"];

    public enum Announcer
    {
        Male,
        Female,
        Random
    }
}
