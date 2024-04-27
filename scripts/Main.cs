using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaveFunctionCollapse;
using WaveFunctionCollapse.Godot;
using Node= WaveFunctionCollapse.Cell;

public partial class Main : Node3D
{
	[Export] public Button ReuseGridButton;

	[Export] public TextEdit ReuseGridText;

	[Export] public Label CellDebugLabel;

	public static Label CellDebugLabelStatic;

	[Export] public PackedScene TileDebugPrefab;

	[Export] public Button SaveSeedButton;

	public GodotTile[] Tiles;

    [Export] public GodotTiles Configuration;

    [Export] public int GridSizeX = 2;

    [Export] public int GridSizeY = 2;

    [Export] public Node3D Container;

    public bool Enabled = false;

    public bool Paused;

    public Grid Grid;

    public static List<Tile> AllGridTiles;

    public Vector3 Bounds;

    public static int RandomSeed;

    public Random Random;

    [Export] private Label RandomSeedLabel;

    [Export] private LineEdit SeedTextLabel;

    public static PackedScene DebugPrefab;

    [Export] public Button PrintGridButton;

    public override void _Ready()
    {
	    DebugPrefab = TileDebugPrefab;

	    CellDebugLabelStatic = CellDebugLabel;

	    SaveSeedButton.Pressed += () => SeedTextLabel.Text = RandomSeed.ToString();

	    Tiles = Configuration.Tiles;

	    AllGridTiles = Tiles.Select(x => x.ToTile()).ToList();

	    var tile = Tiles.First().Prefab.Instantiate<Node3D>();

	    MeshInstance3D mesh = tile as MeshInstance3D;

	    foreach (var node in tile.GetChildren().Cast<Node3D>())
	    {
		    if (mesh != null) break;

		    mesh = GetMesh(node);
	    }

	    if (mesh == null)
	    {
		    GD.Print("Unable to find bounds for mesh.");
		    return;
	    }

	    Bounds = mesh.GetAabb().Size;

	    PrintGridButton.Pressed += PrintGrid;

	    ReuseGridButton.Pressed += ReuseGrid;
    }

    private MeshInstance3D GetMesh(Node3D parent)
    {
	    MeshInstance3D mesh = parent as MeshInstance3D;

	    foreach (var node in parent.GetChildren().Cast<Node3D>())
	    {
		    if (mesh != null) break;

		    mesh = GetMesh(node);
	    }

	    return mesh;
    }

    private void ReuseGrid()
    {
	    var lines = ReuseGridText.Text.Split('\n').Where(x => !string.IsNullOrEmpty(x));

	    var cleanLines = lines.Select(line => line.Split(" ").Where(item => item != "").Select(int.Parse).ToArray()).Reverse().ToArray();

	    var preset = new int[cleanLines.Length, cleanLines[0].Length];

	    for (int x = 0; x < cleanLines[0].Length; x++)
	    {
		    for (int y = 0; y < cleanLines.Length; y++)
		    {
			    preset[y, x] = cleanLines[y][x];
		    }
	    }

	    Grid = new Grid(preset.GetLength(1),
		    preset.GetLength(0),
		    AllGridTiles.ToArray(),
		    preset);

	    foreach (var cell in Grid.Cells)
	    {
		    if (!cell.IsCollapsed) continue;

		    SimpleNeighbourConnectorStrategy.Process(cell);
	    }

	    RenderGrid(Grid);
    }

    public void PrintGrid()
    {
	    if (Grid == null) return;

	    var xLen = Grid.GridSizeX;

	    var yLen = Grid.GridSizeY;

	    var sb = new StringBuilder();

	    for (int y = yLen - 1; y >= 0; y--)
	    {
		    for (int x = 0; x < xLen; x++)
		    {
			    var cell = Grid[x, y];

			    sb.Append($"{AllGridTiles.IndexOf(cell.Options[0]), 3} ");
		    }

		    sb.Append('\n');
	    }

	    ReuseGridText.Text = sb.ToString();
    }

    public override void _Process(double delta)
    {
	    if (Grid == null) return;

	    if (Paused) return;

	    var result = Algorithm.Step(Grid);

	    if (!result)
	    {
		    return;
	    }

	    RenderGrid(Grid);
    }

    public override void _Input(InputEvent @event)
    {
	    if (Input.IsActionJustPressed("generate"))
	    {
		    RandomSeed = new Random().Next();

		    if (SeedTextLabel.Text != "")
		    {
			    RandomSeed = int.Parse(SeedTextLabel.Text);
		    }

		    RandomSeedLabel.Text = RandomSeed.ToString();

		    Random = new Random(RandomSeed);

		    Algorithm.Random = Random;

		    Grid = new Grid(GridSizeX, GridSizeY, AllGridTiles.ToArray());

		    if (AllGridTiles.Any(x => x.Connectors.Count(x => x == "EMPTY") != 0))
		    {
			    BorderStrategy.Process(Grid);
		    }
	    }

	    if (Input.IsActionJustPressed("next"))
	    {
		    Algorithm.Step(Grid);

		    RenderGrid(Grid);
	    }

	    if (Input.IsActionJustPressed("pause"))
	    {
		    Paused = !Paused;
	    }

	    if (Input.IsActionJustPressed("clear"))
	    {
		    Grid = null;

		    ClearContainer();
	    }

	    if (Input.IsActionJustPressed("debug"))
	    {
		    TileDebug.Debug = !TileDebug.Debug;
	    }
    }

    private void ClearContainer()
    {
	    for(int i = 0; i < Container.GetChildCount(); i++)
	    {
		    Container.GetChild(i).QueueFree();
	    }
    }

    private void RenderGrid(Grid grid)
	{
		ClearContainer();

		Renderer.RenderGrid(Tiles, grid, Container, Bounds);
	}

    public static void ShowCellDebug(Cell cell)
	{
		CellDebugLabelStatic.Text = $"Entropy: {cell.Entropy}\n" +
		                            $"Collapsed: {cell.IsCollapsed}\n" +
		                            $"Broken: {cell.IsBroken}\n" +
		                            $"Top Connectors: {cell.Options.Select(x => x.TopConnectors).ToArray().Join(",")}\n" +
		                            $"Bottom Connectors: {cell.Options.Select(x => x.BottomConnectors).ToArray().Join(",")}\n" +
		                            $"Right Connectors: {cell.Options.Select(x => x.RightConnectors).ToArray().Join(",")}\n" +
		                            $"Left Connectors: {cell.Options.Select(x => x.LeftConnectors).ToArray().Join(",")}\n";
	}

	public static void ClearCellDebug()
	{
		CellDebugLabelStatic.Text = "";
	}
}
