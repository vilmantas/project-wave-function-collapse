using Godot;

namespace WaveFunctionCollapse.Godot;

[GlobalClass, Tool]
public partial class GodotTile : Resource
{
    [Export] public int Id = new RandomNumberGenerator().RandiRange(0, 1000000);
    [Export] public PackedScene Prefab;
    [Export] public string TopConnectors;
    [Export] public string RightConnectors;
    [Export] public string BottomConnectors;
    [Export] public string LeftConnectors;
    [Export] public bool GenerateRotations;

    [Export] public bool LimitEnabled;
    [Export] public int Limit = 1;

    [Export] public PositionMode Positioning = PositionMode.All;

    [Export] public int RotationY;

    [Export] public int Weight { get; set; } = 1;

    public Tile ToTile()
    {
        return new Tile
        {
            Name = ResourcePath,
            TopConnectors = TopConnectors,
            RightConnectors = RightConnectors,
            BottomConnectors = BottomConnectors,
            LeftConnectors = LeftConnectors,
            RotationY = RotationY,
            Weight = Weight,
            Limit = Limit,
            LimitEnabled = LimitEnabled,
            Positioning = Positioning,
        };
    }

    public GodotTile Copy()
    {
        return new GodotTile
        {
            ResourcePath = ResourcePath,
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
            Positioning = Positioning,
        };
    }
}