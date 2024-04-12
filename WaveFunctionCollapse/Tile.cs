namespace WaveFunctionCollapse;

public class Tile
{
    public string Name { get; set; }

    public string TopConnectors;

    public string RightConnectors;

    public string BottomConnectors;

    public string LeftConnectors;

    public int Rotation;

    public int Weight { get; set; } = 1;
}