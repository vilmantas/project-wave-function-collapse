using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using WaveFunctionCollapse;
using Node= WaveFunctionCollapse.Cell;

public partial class Main : Node3D
{
	public GodotTile[] Tiles;

    [Export] public GodotTiles Configuration;

    [Export] public int GridSizeX = 2;

    [Export] public int GridSizeY = 2;

    [Export] public Node3D Container;

    public bool Enabled = false;

    public bool Paused;

    public override void _Ready()
    {
	    Tiles = Configuration.Tiles;

	    var iterations = Tiles.Length;

	    var newTiles = Tiles.ToList();

	    for (int i = 0; i < iterations; i++)
	    {
		    var godotTile = Tiles[i];

		    if (!godotTile.GenerateRotations) continue;

		    newTiles.AddRange(AppendRotations(godotTile));
	    }

	    Tiles = newTiles.ToArray();
    }

    private List<GodotTile> AppendRotations(GodotTile tile)
    {
	    var result = new List<GodotTile>();

	    var connections = new List<string>()
	    {
		    tile.TopConnectors, tile.RightConnectors,
		    tile.BottomConnectors, tile.LeftConnectors
	    };

	    for (int i = 1; i < 4; i++)
	    {
		    var lastElement = connections[^1];
		    connections.RemoveAt(connections.Count - 1);
		    connections.Insert(0, lastElement);

		    var x = new GodotTile();

		    x.TopConnectors = connections[0];
		    x.RightConnectors = connections[1];
		    x.BottomConnectors = connections[2];
		    x.LeftConnectors = connections[3];

		    x.Rotation = i;
		    x.Prefab = tile.Prefab;
		    x.GenerateRotations = false;
		    x.Weight = tile.Weight;

		    result.Add(x);
	    }

	    return result;
    }

    public override void _Process(double delta)
    {
	    if (Paused) return;

    }

    public override void _Input(InputEvent @event)
    {
	    int randomSeed = 2;

	    if (Input.IsActionJustPressed("generate"))
	    {
		    var random = new Random();

		    Grid grid = new Grid(GridSizeX, GridSizeY,
			    Tiles.Select(x => x.ToTile()).ToArray());

		    while (!grid.IsCollapsed)
		    {
			    var x = grid.LowestEntropyNodes();

			    var randomNode = x[random.Next(x.Length)];

			    if (randomNode.Entropy == 0) break;

			    randomNode.Collapse();

			    SimpleNeighbourValidator.Process(randomNode);
		    }

		    RenderGrid(grid);
	    }

	    if (Input.IsActionJustPressed("pause"))
	    {
		    Paused = !Paused;
	    }

	    if (Input.IsActionJustPressed("clear"))
	    {
		    for(int i = 0; i < Container.GetChildCount(); i++)
		    {
			    Container.GetChild(i).QueueFree();
		    }
	    }
    }

	private void RenderGrid(Grid grid)
	{
		for(int i = 0; i < Container.GetChildCount(); i++)
		{
			Container.GetChild(i).QueueFree();
		}

		foreach (var node in grid.Cells)
		{
			if (!node.IsCollapsed)
			{
				var t = Tiles[0].Prefab.Instantiate<TileDebug>();
				Container.AddChild(t);

				t.ValidOptions = node.Entropy;
				t.X = node.X;
				t.Y = node.Y;

				t.GlobalPosition = new Vector3(node.X, 0, -node.Y);
			}
			else
			{
				var tile = FindTile(node.Options[0].Name).Prefab.Instantiate<Node3D>();

				if (tile is TileDebug debugTile)
				{
					debugTile.ValidOptions = 1;
					debugTile.X = node.X;
					debugTile.Y = node.Y;
					debugTile.Up = node.Up != null;
					debugTile.Right = node.Right != null;
					debugTile.Down = node.Down != null;
					debugTile.Left = node.Left != null;
				}

				Container.AddChild(tile);

				tile.GlobalPosition = new Vector3(node.X, 0, -node.Y);
				tile.RotationDegrees = new Vector3(0, -90 * node.Options[0].Rotation, 0);
			}
		}
	}

	private GodotTile FindTile(string name) => Tiles.First(x => x.Prefab.ResourcePath == name);
}
