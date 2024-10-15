using System;
using Godot;

namespace Party.Game.Experience.Events;

public sealed partial class GameEventTriviaMath : GameEventTrivia
{
    public override double Duration => 10.0;

    private int a;
    private int b;
    private int correct;

    protected override void Init()
    {
        int opa = Random.Shared.Next(minimum, maximum);
        int opb = Random.Shared.Next(minimum, maximum);
        var opr = Random.Shared.NextBoolean() ? Operator.Add : Operator.Sub;
        correct = opr is Operator.Add ? opa + opb : opa - opb;

        if (Random.Shared.NextBoolean())
        {
            a = correct;
            b = Random.Shared.NextBoolean() ? correct + Random.Shared.Next(minimum, deviate) : correct - Random.Shared.Next(minimum, deviate);
        }
        else
        {
            a = Random.Shared.NextBoolean() ? correct + Random.Shared.Next(minimum, deviate) : correct - Random.Shared.Next(minimum, deviate);
            b = correct;
        }

        GD.Print(a, " ", b, " ", correct);

        GetNode<Label>("%A").Text = a.ToString();
        GetNode<Label>("%B").Text = b.ToString();
        GetNode<Label>("%Q").Text = string.Format("{0} {2} {1} = ?", opa, opb, opr is Operator.Add ? '+' : '-');
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

    private enum Operator
    {
        Add,
        Sub,
    }

    private const int maximum = 100;
    private const int minimum = 0;
    private const int deviate = 25;
}