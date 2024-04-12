using Godot;

namespace WaveFunctionCollapse.Godot;

[GlobalClass]
public partial class GodotTile : Resource
{
    [Export] public PackedScene Prefab;
    [Export] public string TopConnectors;
    [Export] public string RightConnectors;
    [Export] public string BottomConnectors;
    [Export] public string LeftConnectors;
    [Export] public bool GenerateRotations;

    [Export] public bool LimitEnabled;
    [Export] public int Limit = 1;

    public int RotationY;

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
            Rotation = RotationY,
            Weight = Weight,
            Limit = Limit,
            LimitEnabled = LimitEnabled,
        };
    }

    public GodotTile Copy()
    {
        return new GodotTile
        {
            Prefab = Prefab,
            TopConnectors = TopConnectors,
            RightConnectors = RightConnectors,
            BottomConnectors = BottomConnectors,
            LeftConnectors = LeftConnectors,
            GenerateRotations = GenerateRotations,
            RotationY = RotationY,
            Weight = Weight,
            Limit = Limit,
            LimitEnabled = LimitEnabled,
        };
    }
}