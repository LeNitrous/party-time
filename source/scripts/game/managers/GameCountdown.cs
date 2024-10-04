using System;
using System.IO;
using Godot;
using NathanHoad;

namespace Party.Game.Experience.Managers;

public sealed partial class GameCountdown : Node
{
    private Timer time;
    private Timer wait;
    private bool isCountdownActive;

    public override void _Ready()
    {
        time = GetNode<Timer>("Time");
        wait = GetNode<Timer>("Wait");
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

            string name = countdown[index--];
            string path = Path.Combine("res://", "sounds", "voice", "b", name);
            SoundManager.PlaySound(GD.Load<AudioStream>(path), "Voice");
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

    private static readonly string[] countdown = new string[]
    {
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
    };
}
