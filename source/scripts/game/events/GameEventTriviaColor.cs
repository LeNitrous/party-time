using System;
using Godot;

namespace Party.Game.Experience.Events;

public sealed partial class GameEventTriviaColor : GameEventTrivia
{
    public override double Duration => 6.0;

    private RainbowColor a;
    private RainbowColor b;
    private RainbowColor correct;

    protected override void Init()
    {
        correct = getRandomColor();

        if (Random.Shared.NextBoolean())
        {
            a = correct;
            b = getIncorrect(correct);
        }
        else
        {
            a = getIncorrect(correct);
            b = correct;
        }

        GetNode<Control>("%L/Modulate").Modulate = getColor(a).WithAlpha(0.2f);
        GetNode<Control>("%R/Modulate").Modulate = getColor(b).WithAlpha(0.2f);
        GetNode<Label>("%Q").Text = $"{Tr("GAME_HINT_TRIVIA_COLOR")} {Tr(getLocalized(correct))}!";
    }

    protected override bool IsCorrect(int index)
    {
        if (index == 0)
        {
            return correct == a;
        }

        if (index == 1)
        {
            return correct == b;
        }

        return false;
    }

    private static Color getColor(RainbowColor color)
    {
        return color switch
        {
            RainbowColor.Red    => Colors.Red,
            RainbowColor.Orange => Colors.Orange,
            RainbowColor.Yellow => Colors.Yellow,
            RainbowColor.Green  => Colors.Green,
            RainbowColor.Blue   => Colors.Blue,
            RainbowColor.Indigo => Colors.Indigo,
            RainbowColor.Violet => Colors.Violet,
            _ => throw new ArgumentOutOfRangeException(nameof(color)),
        };
    }

    private static string getLocalized(RainbowColor color)
    {
        return color switch
        {
            RainbowColor.Red    => "GAME_HINT_TRIVIA_COLOR_RED",
            RainbowColor.Orange => "GAME_HINT_TRIVIA_COLOR_ORANGE",
            RainbowColor.Yellow => "GAME_HINT_TRIVIA_COLOR_YELLOW",
            RainbowColor.Green  => "GAME_HINT_TRIVIA_COLOR_GREEN",
            RainbowColor.Blue   => "GAME_HINT_TRIVIA_COLOR_BLUE",
            RainbowColor.Indigo => "GAME_HINT_TRIVIA_COLOR_INDIGO",
            RainbowColor.Violet => "GAME_HINT_TRIVIA_COLOR_VIOLET",
            _ => throw new ArgumentOutOfRangeException(nameof(color)),
        };
    }

    private static RainbowColor getRandomColor()
    {
        return (RainbowColor)Random.Shared.Next(0, (int)RainbowColor.Maximum);
    }

    private static RainbowColor getIncorrect(RainbowColor correct)
    {
        RainbowColor answer;
        
        do
        {
            answer = getRandomColor();
        }
        while (answer == correct);

        return answer;
    }

    private enum RainbowColor
    {
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Indigo,
        Violet,
        Maximum,
    }
}
