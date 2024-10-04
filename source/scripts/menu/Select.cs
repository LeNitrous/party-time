using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Party.Game.Menu;

[Tool]
public partial class Select : Interactable
{
    [Export]
    public string Text
    {
        get => this.GetValueAt<string>("%Label", Label.PropertyName.Text);
        set => this.SetValueAt("%Label", Label.PropertyName.Text, value);
    }

    [Export]
    public Array<string> Options
    {
        get => options;
        set => setOptions(in value);
    }

    [Export]
    public int Selected
    {
        get => selected;
        set => setSelected(in value);
    }

    private bool primed;
    private int selected;
    private Array<string> options = new Array<string>();
    private Control flows;
    private PackedScene packs;
    private readonly List<SelectOption> items = new List<SelectOption>();

    public override void _Ready()
    {
        updateOptions();
        updateSelection();
    }

    public override void _ValidateProperty(Dictionary property)
    {
        if (property["name"].AsStringName() == PropertyName.Selected)
        {
            property["hint"] = (int)(property["hint"].As<PropertyHint>() | PropertyHint.Range);
            property["hint_string"] = $"0,{Options.Count - 1}";
        }
    }

    protected override void OnStateChanged(State state)
    {
        bool active = state.HasFlag(State.Selected) || state.HasFlag(State.Hovered);

        foreach (var item in items)
        {
            item.Highligted = item.Selected && active;
        }
    }

    private void onSelectGUIInput(InputEvent e)
    {
        if (e is InputEventMouseButton b)
        {
            if (b.ButtonIndex == MouseButton.Left)
            {
                if (b.Pressed)
                {
                    primed = true;
                }

                if (primed && !b.Pressed)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (!items[i].GetGlobalRect().HasPoint(b.GlobalPosition))
                        {
                            continue;
                        }

                        setSelected(i);
                    }

                    primed = false;
                    CallDeferred(MethodName.ReleaseFocus);
                }
            }
        }

        if (e is InputEventMouseMotion m)
        {
            if (primed && !GetGlobalRect().HasPoint(m.GlobalPosition))
            {
                primed = false;
                CallDeferred(MethodName.ReleaseFocus);
            }
        }

        if (e.IsAction("ui_left"))
        {
            prev();
        }

        if (e.IsAction("ui_right"))
        {
            next();
        }
    }

    private void next()
    {
        step(+1);
    }

    private void prev()
    {
        step(-1);
    }

    private void step(int direction)
    {
        int next = Selected + direction;

        if (next >= items.Count)
        {
            next = 0;
        }

        if (next <= 0)
        {
            next = items.Count - 1;
        }

        Selected = next;
    }

    private void setOptions(in Array<string> value)
    {
        if (value is not null && options is not null && options.RecursiveEqual(value))
        {
            return;
        }

        int prevLength = value is not null ? value.Count : 0;
        int nextLength = options is not null ? options.Count : 0;

        options = value;

        if (IsNodeReady())
        {
            updateOptions(prevLength != nextLength);
        }
    }

    private void setSelected(in int value)
    {
        if (selected == value)
        {
            return;
        }

        selected = value;

        if (IsNodeReady())
        {
            updateSelection();
        }

        EmitSignal(SignalName.SelectionChanged, selected);
    }

    private void updateSelection()
    {
        if (selected >= items.Count)
        {
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].Selected = selected == i;
        }
    }

    private void updateOptions(bool lengthChanged = false)
    {
        flows ??= GetNode<Control>("%Flow");
        packs ??= ResourceLoader.Load<PackedScene>("res://scenes/components/ui_button_option.tscn");
        items.Clear();

        foreach (var child in flows.GetChildren())
        {
            child.QueueFree();
        }

        if (options is null)
        {
            return;
        }

        for (int i = 0; i < options.Count; i++)
        {
            string option = options[i];

            var scene = packs.Instantiate<SelectOption>();
            scene.Text = option;
            scene.Selected = selected == i;
            scene.Highligted = false;
            scene.SizeFlagsVertical = SizeFlags.ShrinkCenter;
            scene.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            items.Add(scene);
            flows.AddChild(scene);
        }

        if (lengthChanged)
        {
            if (items.Count > 0)
            {
                setSelected(Math.Clamp(selected, 0, items.Count - 1));
            }

            NotifyPropertyListChanged();
        }
    }

    [Signal]
    public delegate void SelectionChangedEventHandler(int index);
}
