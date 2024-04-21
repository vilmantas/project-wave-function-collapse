using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using WaveFunctionCollapse;
using WaveFunctionCollapse.Godot;
using Node= WaveFunctionCollapse.Cell;

public partial class Main : Node3D
{
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

    public List<Tile> AllGridTiles;

    public Vector3 Bounds;

    public static int RandomSeed;

    public Random Random;

    [Export] private Label RandomSeedLabel;

    [Export] private LineEdit SeedTextLabel;

    public static PackedScene DebugPrefab;

    public override void _Ready()
    {
	    DebugPrefab = TileDebugPrefab;

	    CellDebugLabelStatic = CellDebugLabel;

	    SaveSeedButton.Pressed += () => SeedTextLabel.Text = RandomSeed.ToString();

	    Tiles = Configuration.Tiles;

	    var gridTiles = Tiles.Select(x => x.ToTile()).ToList();

	    var rotatable = Tiles.Where(x => x.GenerateRotations).Select(x => x.ToTile());

	    var rotations = Utilities.GenerateRotations(rotatable.ToArray());

	    gridTiles.AddRange(rotations);

	    AllGridTiles = gridTiles;

	    var tile = Tiles.First().Prefab.Instantiate<Node3D>();

	    var mesh = tile.GetNode<MeshInstance3D>("model");

	    Bounds = mesh.GetAabb().Size;
    }

    public override void _Process(double delta)
    {
	    if (Grid == null) return;

	    if (Paused) return;

	    var result = Algorithm.Step(Grid);

	    if (!result)
	    {
		    Grid = null;
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
