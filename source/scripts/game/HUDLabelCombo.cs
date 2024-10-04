using Godot;

namespace Party.Game.Experience;

public sealed partial class HUDLabelCombo : MarginContainer
{
    private bool isCombo;

    private void onValueChanged(Variant value)
    {
        if (!isCombo && value.AsInt32() > 1)
        {
            var tween = GetTree().CreateTween();
            tween.SetTrans(Tween.TransitionType.Quad);
            tween.SetEase(Tween.EaseType.Out);
            tween.TweenMethod(Callable.From((int value) => AddThemeConstantOverride("margin_bottom", value)), -200, 0, 0.5);
            tween.Play();
            isCombo = true;
        }

        if (isCombo && value.AsInt32() <= 0)
        {
            var tween = GetTree().CreateTween();
            tween.SetTrans(Tween.TransitionType.Quad);
            tween.SetEase(Tween.EaseType.Out);
            tween.TweenMethod(Callable.From((int value) => AddThemeConstantOverride("margin_bottom", value)), 0, -200, 0.5);
            tween.Play();
            isCombo = false;
        }
    }
}
