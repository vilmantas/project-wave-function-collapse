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

    public Tile ToTile()
    {
        return new Tile
        {
            Name = Prefab.ResourcePath,
            TopConnectors = TopConnectors,
            RightConnectors = RightConnectors,
            BottomConnectors = BottomConnectors,
            LeftConnectors = LeftConnectors
        };
    }
}
