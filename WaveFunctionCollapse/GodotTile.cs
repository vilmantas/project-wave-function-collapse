using Godot;
using WaveFunctionCollapse;

[GlobalClass]
public partial class GodotTile : Resource
{
    [Export] public PackedScene Prefab;

    [Export] public string TopConnectors;

    [Export] public string RightConnectors;

    [Export] public string BottomConnectors;

    [Export] public string LeftConnectors;

    [Export] public bool GenerateRotations;

    public int Rotation;

    [Export] public int Weight { get; set; } = 1;

    public Tile ToTile()
    {
        return new Tile
        {
            Name = Prefab.ResourcePath,
            TopConnectors = TopConnectors,
            RightConnectors = RightConnectors,
            BottomConnectors = BottomConnectors,
            LeftConnectors = LeftConnectors,
            Rotation = Rotation,
            Weight = Weight,
        };
    }
}
