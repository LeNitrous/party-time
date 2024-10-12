using Godot;

namespace Party.Game.Experience;

[GlobalClass]
public partial class GameEventMetadata : Resource
{
    public string Name => name;

    public string Description => description;

    public PackedScene Scene => scene;

    public double Weight => weight;

    [Export]
    private string name;

    [Export(PropertyHint.MultilineText)]
    private string description;

    [Export]
    private PackedScene scene;

    [Export]
    private double weight;
}
