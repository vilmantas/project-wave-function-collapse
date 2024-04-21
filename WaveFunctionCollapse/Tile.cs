using System.Linq;

namespace WaveFunctionCollapse;

public class Tile
{
    public string Name;
    public string TopConnectors;
    public string RightConnectors;
    public string BottomConnectors;
    public string LeftConnectors;
    public int RotationY;
    public int Weight = 1;

    public bool LimitEnabled;
    public int Limit;

    public PositionMode Positioning = PositionMode.All;

    public string[] Connectors => new[]
        {TopConnectors, RightConnectors, BottomConnectors, LeftConnectors};

    public bool IsCorner => Connectors.Count(x => x == "EMPTY") == 2;
    public bool IsBorder => Connectors.Count(x => x == "EMPTY") == 1;

    public Tile Copy()
    {
        return new Tile
        {
            Name = Name,
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
}

public enum PositionMode
{
    All,
    Center,
    Border,
    Corner,
}