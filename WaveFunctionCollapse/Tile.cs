namespace WaveFunctionCollapse;

public class Tile
{
    public string Name;
    public string TopConnectors;
    public string RightConnectors;
    public string BottomConnectors;
    public string LeftConnectors;
    public int Rotation;
    public int Weight = 1;

    public bool LimitEnabled;
    public int Limit;

    public PositionMode Positioning = PositionMode.All;

    public string[] Connectors => new[]
        {TopConnectors, RightConnectors, BottomConnectors, LeftConnectors};
}

public enum PositionMode
{
    All,
    Center,
    Border,
    Corner,
}