using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using WaveFunctionCollapse;
using Node= WaveFunctionCollapse.Cell;

public partial class Main : Node3D
{
    [Export] public GodotTile[] Tiles;

    [Export] public int GridSizeX = 2;

    [Export] public int GridSizeY = 2;

    [Export] public Node3D Container;

    public bool Enabled = false;

    public bool Paused;

    private void Magic(Node cell)
    {
	    if (cell == null) return;

	    var validOptions = new List<string>();

	    if (cell.Up != null)
	    {
		    validOptions = cell.Up.Options.Select(x => x.BottomConnectors).ToList();

		    cell.Options.RemoveAll(x => !validOptions.Contains(x.TopConnectors));
	    }

	    if (cell.Down != null)
	    {
		    validOptions = cell.Down.Options.Select(x => x.TopConnectors).ToList();

		    cell.Options.RemoveAll(x => !validOptions.Contains(x.BottomConnectors));
	    }

	    if (cell.Left != null)
	    {
		    validOptions = cell.Left.Options.Select(x => x.RightConnectors).ToList();

		    cell.Options.RemoveAll(x => !validOptions.Contains(x.LeftConnectors));
	    }

	    if (cell.Right != null)
	    {
		    validOptions = cell.Right.Options.Select(x => x.LeftConnectors).ToList();

		    cell.Options.RemoveAll(x => !validOptions.Contains(x.RightConnectors));
	    }
    }

    public override void _Process(double delta)
    {
	    if (Paused) return;

	    if (Grid == null) return;

	    var allNodes = new List<Node>();

	    foreach (var row in Grid)
	    {
		    allNodes.AddRange(row);
	    }

	    if (allNodes.Any(x => !x.IsCollapsedOld))
	    {
		    ProcessGrid(Grid);

		    // RenderGrid(Grid);
	    }
    }

    public override void _Input(InputEvent @event)
    {
	    int randomSeed = 5;

	    if (Input.IsActionJustPressed("generate"))
	    {
		    var random = new Random(randomSeed);

		    Grid grid = new Grid(GridSizeX, GridSizeY,
			    Tiles.Select(x => x.ToTile()).ToArray());

		    var cell = grid[grid.Cells.Length / 2];

		    cell.Left.Collapse(randomSeed);

		    while (!grid.IsCollapsed)
		    {
			    var x = grid.LowestEntropyNodes();

			    var randomNode = x[random.Next(x.Length)];

			    if (randomNode.Entropy == 0) break;

			    randomNode.Collapse(1);

			    Magic(randomNode.Up);
			    Magic(randomNode.Down);
			    Magic(randomNode.Left);
			    Magic(randomNode.Right);
		    }

		    RenderGrid(grid);
	    }

	    if (Input.IsActionJustPressed("pause"))
	    {
		    Paused = !Paused;
	    }

	    if (Input.IsActionJustPressed("clear"))
	    {
		    Grid = null;

		    for(int i = 0; i < Container.GetChildCount(); i++)
		    {
			    Container.GetChild(i).QueueFree();
		    }
	    }
    }

    private Node[][] Grid;

    private void DoMagic()
	{
		Grid = GetEmptyGrid();

		var allNodes = new List<Node>();

		foreach (var row in Grid)
		{
			allNodes.AddRange(row);
		}

		while (allNodes.Any(x => !x.IsCollapsedOld))
		{
			ProcessGrid(Grid);
		}

		// RenderGrid(Grid);
	}

	private void ProcessGrid(Node[][] grid)
	{
		var allNodes = new List<Node>();

		foreach (var row in Grid)
		{
			allNodes.AddRange(row);
		}

		var random = new Random();

		allNodes = allNodes.Where(x => !x.IsCollapsedOld).OrderBy(x => x.OptionsOld.Count).ToList();

		allNodes = allNodes.Where(x => x.OptionsOld.Count == allNodes[0].OptionsOld.Count).ToList();

		var randomNode = allNodes[random.Next(allNodes.Count)];

		randomNode.Collapse();

		for (int y = 0; y < grid.Length; y++)
		{
			for (int xx = 0; xx < grid[y].Length; xx++)
			{
				var node = grid[y][xx];

				if (node.IsCollapsedOld) continue;

				var upperNode = y > 0 ? grid[y - 1][xx] : null;

				if (upperNode != null)
				{
					var validOptions = new List<string>();
					foreach (var opt in upperNode.OptionsOld)
					{
						var valid = new List<string>();//opt.Down.Select(x => x.ResourcePath);
						validOptions.AddRange(valid);
					}
					node.OptionsOld.RemoveAll(x => !validOptions.Contains(x.Prefab.ResourcePath));
				}

				var leftNode = xx > 0 ? grid[y][xx - 1] : null;

				if (leftNode != null)
				{
					var validOptions = new List<string>();
					foreach (var opt in leftNode.OptionsOld)
					{
						var valid = new List<string>();//opt.Right.Select(x => x.ResourcePath);
						validOptions.AddRange(valid);
					}
					node.OptionsOld.RemoveAll(x => !validOptions.Contains(x.Prefab.ResourcePath));
				}

				var rightNode = xx < grid[y].Length - 1 ? grid[y][xx + 1] : null;

				if (rightNode != null)
				{
					var validOptions = new List<string>();
					foreach (var opt in rightNode.OptionsOld)
					{
						var valid = new List<string>();//opt.Left.Select(x => x.ResourcePath);
						validOptions.AddRange(valid);
					}
					node.OptionsOld.RemoveAll(x => !validOptions.Contains(x.Prefab.ResourcePath));
				}


				var downNode = y < grid.Length - 1 ? grid[y + 1][xx] : null;

				if (downNode != null)
				{
					var validOptions = new List<string>();
					foreach (var opt in downNode.OptionsOld)
					{
						var valid = new List<string>();//opt.Top.Select(x => x.ResourcePath);
						validOptions.AddRange(valid);
					}
					node.OptionsOld.RemoveAll(x => !validOptions.Contains(x.Prefab.ResourcePath));
				}
			}
		}
	}

	private Node[][] GetEmptyGrid()
	{
		Node[][] grid = new Node[GridSizeY][];

		for (var x = 0; x < GridSizeY; x++)
		{
			var row = new Node[GridSizeX];
			grid[x] = row;

			for (var i = 0; i < GridSizeX; i++)
			{
				var node = new Node();

				node.IsCollapsedOld = false;

				node.OptionsOld = new List<GodotTile>();

				foreach (var origTile in Tiles)
				{
					node.OptionsOld.Add(origTile);
				}

				row[i] = node;
			}
		}

		return grid;
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
				Container.AddChild(tile);

				tile.GlobalPosition = new Vector3(node.X, 0, -node.Y);
			}
		}
	}

	private GodotTile FindTile(string name) => Tiles.First(x => x.Prefab.ResourcePath == name);
}
