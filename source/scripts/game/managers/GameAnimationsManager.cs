using System;
using Godot;

namespace Party.Game.Experience.Managers;

public sealed partial class GameAnimationsManager : Node
{
    private Node borderFlashEffect;
    private Timer time;
    private Control spinner;
    private Control hudMarginH;
    private Control hudMarginV;
    private Control hudMarginU;
    private Control hudMarginS;
    private Label timeLabel;

    public override void _Ready()
    {
        time = GetNode<Timer>("%Context/Time");
        spinner = GetNode<TextureRect>("%Spinner");
        timeLabel = GetNode<Label>("%HUD/H Margin/Time");
        hudMarginH = GetNode<Control>("%HUD/H Margin");
        hudMarginV = GetNode<Control>("%HUD/V Margin");
        hudMarginU = GetNode<Control>("%HUD/U Margin");
        hudMarginS = GetNode<Control>("%HUD/S Margin");
        borderFlashEffect = GetNode("%HUD/Border/Flash");
    }

    public override void _Process(double delta)
    {
        timeLabel.Text = (time.IsStopped() || time.Paused) ? string.Empty : TimeSpan.FromSeconds(Mathf.Ceil(time.TimeLeft)).ToString("%s");
    }

    private void onPhaseChanged(Phase phase)
    {
        if (phase == Phase.Prologue)
        {
            var spin = CreateTween();

            spin
                .SetLoops()
                .TweenProperty(spinner, "rotation_degrees", 360.0, 0.5)
                .AsRelative();
            
            spin.Play();

            var load = CreateTween();

            load
                .SetParallel();

            load
                .TweenCallback(Callable.From(() => hudMarginS.GetNode(shrinker).Call(Tween.MethodName.Play)))
                .SetDelay(GameContext.Duration.Prologue - 5.0);

            load
                .TweenProperty(spinner, "modulate", Colors.Transparent, 0.25)
                .SetDelay(GameContext.Duration.Prologue - 5.0);

            load.Play();
        }

        if (phase == Phase.Starting)
        {
            borderFlashEffect.GetParent().GetNode(border)
                .Set(CanvasItem.PropertyName.SelfModulate, Colors.White);

            borderFlashEffect.Call(Tween.MethodName.Play);

            hudMarginH.GetNode(shrinker).Call(Tween.MethodName.Play);
            hudMarginV.GetNode(shrinker).Call(Tween.MethodName.Play);
            hudMarginU.GetNode(expander).Call(Tween.MethodName.Play);
            hudMarginS.GetNode(expander).Call(Tween.MethodName.Play);
        }

        if (phase == Phase.Ending)
        {
            hudMarginH.GetNode(expander).Call(Tween.MethodName.Play);
            hudMarginV.GetNode(expander).Call(Tween.MethodName.Play);
            hudMarginU.GetNode(shrinker).Call(Tween.MethodName.Play);
            hudMarginS.GetNode(shrinker).Call(Tween.MethodName.Play);
        }

        if (phase == Phase.Epilogue)
        {
            var tween = CreateTween();

            tween
                .TweenProperty(hudMarginU.GetNode<Control>(modulate), "modulate", grid_modulate_default, 0.5);
            
            tween.Play();

            hudMarginS.GetNode(expander).Call(Tween.MethodName.Play);
        }
    }

    private void onCompletionChanged(Completion state)
    {
        bool hasWonRound = state is Completion.Win or Completion.WinTimeout;

        borderFlashEffect.GetParent().GetNode(border)
            .Set(CanvasItem.PropertyName.SelfModulate, hasWonRound ? grid_modulate_winning : grid_modulate_loseing);

        borderFlashEffect.Call(Tween.MethodName.Play);

        hudMarginU.GetNode<Control>(modulate).Modulate = new Color(hasWonRound ? grid_modulate_winning : grid_modulate_loseing, 0.43f);
    }

    private static readonly NodePath border = "Visual";
    private static readonly NodePath expander = "Expander";
    private static readonly NodePath shrinker = "Shrinker";
    private static readonly NodePath modulate = "Modulator";
    private static readonly Color grid_modulate_default = Color.Color8(0, 179, 255, 110);
    private static readonly Color grid_modulate_winning = Color.Color8(89, 255, 117);
    private static readonly Color grid_modulate_loseing = Color.Color8(255, 89, 89);
}
