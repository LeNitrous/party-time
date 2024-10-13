using System;
using Godot;
using Godot.Collections;
using NathanHoad;

namespace Party.Game.Menu;

public abstract partial class Choice : Interactable
{
    [Export]
    public Array<string> Options
    {
        get => option;
        set => onOptionChanged(ref option, in value);
    }

    [Export]
    public int Selected
    {
        get => select;
        set => onSelectChanged(ref select, in value);
    }

    private int select = -1;
    private AudioStream effect;
    private Array<string> option = [];

    public override void _Ready()
    {
        effect = GD.Load<AudioStream>("res://sounds/effects/ui_click.ogg");
        OnSelectChanged(select);
        OnOptionChanged(option);
    }

    public override void _ValidateProperty(Dictionary property)
    {
        if (property["name"].AsStringName() == PropertyName.Selected)
        {
            property["hint"] = (int)(property["hint"].As<PropertyHint>() | PropertyHint.Range);
            property["hint_string"] = $"0,{Options.Count - 1}";
        }
    }

    public void Next()
    {
        step(+1);
    }

    public void Prev()
    {
        step(-1);
    }

    protected virtual void OnSelectChanged(int value)
    {
    }

    protected virtual void OnOptionChanged(Array<string> value)
    {
    }

    private void step(int direction)
    {
        int next = Selected + direction;

        if (next >= Options.Count)
        {
            next = 0;
        }

        if (next < 0)
        {
            next = Options.Count - 1;
        }

        Selected = next;
    }

    private void onSelectChanged(ref int field, in int value)
    {
        if (Options is null || value >= Options.Count)
        {
            return;
        }

        if (field == value)
        {
            return;
        }

        field = value;

        if (value >= 0)
        {
            if (IsNodeReady())
            {
                OnSelectChanged(value);
                SoundManager.PlayUISoundWithPitch(effect, Mathf.Remap(Random.Shared.NextSingle(), 0.0f, 1.0f, 0.8f, 1.2f), "Effects");
            }

            EmitSignal(SignalName.SelectionChanged, select);
        }
    }

    private void onOptionChanged(ref Array<string> field, in Array<string> value)
    {
        if (field is not null && value is not null && field.RecursiveEqual(value))
        {
            return;
        }

        int prevLength = field is not null ? field.Count : 0;
        int nextLength = value is not null ? value.Count : 0;

        field = value;

        if (prevLength != nextLength)
        {
            if (nextLength > 0)
            {
                Selected = Math.Clamp(select, 0, nextLength - 1);
            }
            else
            {
                Selected = -1;
            }

            NotifyPropertyListChanged();
        }

        if (IsNodeReady())
        {
            OnOptionChanged(value);
        }
    }

    [Signal]
    public delegate void SelectionChangedEventHandler(int index);
}
