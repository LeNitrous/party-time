using System.Linq;
using Godot;
using Godot.Collections;
using Party.Game.Experience;
using Party.Game.Experience.Directors;

namespace Party.Game.Menu.Play;

public sealed partial class SelectPracticeEvent : Node
{
    private Label label;
    private Choice choice;
    private TextureRect image;
    private Texture2D noPreviewImage;
    private GameEventCollection events;

    public override void _Ready()
    {
        label = GetNode<Label>("%Inform");
        image = GetNode<TextureRect>("%Image");
        events = (GameEventCollection)GD.Load<Resource>(ProjectSettings.GetSetting("application/game/collection", string.Empty).AsString());
        choice = GetNode<Choice>("%Select");
        choice.SelectionChanged += onSelectionChanged;
        choice.Options = new Array<string>(events.Select(e => e.Name).ToArray());
    }

    public void Confirm()
    {
        if (SceneStack.Current is null)
        {
            return;
        }

        GameContext.Director = new GameDirectorPractice(events[choice.Selected].Scene);
        SceneStack.Current.Push("res://scenes/game.tscn");
    }

    private void onSelectionChanged(int index)
    {
        label.Text = events[index].Description;
        image.Texture = events[index].Preview ?? (noPreviewImage ??= GD.Load<Texture2D>("res://textures/menu/no_image.jpg"));
    }
}