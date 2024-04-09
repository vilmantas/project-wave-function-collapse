using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Main : Node3D
{
    [Export] public WcfTile[] Tiles;

    [Export] public int GridSizeX = 2;

    [Export] public int GridSizeY = 2;

    [Export] public Node3D Container;

    public override void _Input(InputEvent @event)
    {
	    if (Input.IsActionJustPressed("rotate"))
	    {
		    DoMagic();
	    }
    }

    private WcfNode[][] Grid;

    private void DoMagic()
	{
		Grid = GetEmptyGrid();

		var allNodes = new List<WcfNode>();

		foreach (var row in Grid)
		{
			allNodes.AddRange(row);
		}

		while (allNodes.Any(x => !x.IsCollapsed))
		{
			ProcessGrid(Grid);
		}

		RenderGrid(Grid);
	}

	private void ProcessGrid(WcfNode[][] grid)
	{
		var allNodes = new List<WcfNode>();

		foreach (var row in Grid)
		{
			allNodes.AddRange(row);
		}

		var random = new Random();

		allNodes = allNodes.Where(x => !x.IsCollapsed).OrderBy(x => x.Options.Count).ToList();

		allNodes = allNodes.Where(x => x.Options.Count == allNodes[0].Options.Count).ToList();

		var randomNode = allNodes[random.Next(allNodes.Count)];

		randomNode.IsCollapsed = true;
		randomNode.Options = new List<WcfTile> { randomNode.Options[random.Next(randomNode.Options.Count)] };

		for (int y = 0; y < grid.Length; y++)
		{
			for (int xx = 0; xx < grid[y].Length; xx++)
			{
				var node = grid[y][xx];

				if (node.IsCollapsed) continue;

				var upperNode = y > 0 ? grid[y - 1][xx] : null;

				if (upperNode != null)
				{
					var validOptions = new List<string>();
					foreach (var opt in upperNode.Options)
					{
						var valid = opt.Down.Select(x => x.ResourcePath);
						validOptions.AddRange(valid);
					}
					node.Options.RemoveAll(x => !validOptions.Contains(x.Tile.ResourcePath));
				}

				var leftNode = xx > 0 ? grid[y][xx - 1] : null;

				if (leftNode != null)
				{
					var validOptions = new List<string>();
					foreach (var opt in leftNode.Options)
					{
						var valid = opt.Right.Select(x => x.ResourcePath);
						validOptions.AddRange(valid);
					}
					node.Options.RemoveAll(x => !validOptions.Contains(x.Tile.ResourcePath));
				}

				var rightNode = xx < grid[y].Length - 1 ? grid[y][xx + 1] : null;

				if (rightNode != null)
				{
					var validOptions = new List<string>();
					foreach (var opt in rightNode.Options)
					{
						var valid = opt.Left.Select(x => x.ResourcePath);
						validOptions.AddRange(valid);
					}
					node.Options.RemoveAll(x => !validOptions.Contains(x.Tile.ResourcePath));
				}


				var downNode = y < grid.Length - 1 ? grid[y + 1][xx] : null;

				if (downNode != null)
				{
					var validOptions = new List<string>();
					foreach (var opt in downNode.Options)
					{
						var valid = opt.Top.Select(x => x.ResourcePath);
						validOptions.AddRange(valid);
					}
					node.Options.RemoveAll(x => !validOptions.Contains(x.Tile.ResourcePath));
				}
			}
		}
	}

	private void CleanOptions(List<WcfTile> options, string[] validOptions)
	{
		// options.RemoveAll(x => !validOptions.Contains(x.Tile.ResourcePath));


	}

	private WcfNode[][] GetEmptyGrid()
	{
		WcfNode[][] grid = new WcfNode[GridSizeY][];

		for (var x = 0; x < GridSizeY; x++)
		{
			var row = new WcfNode[GridSizeX];
			grid[x] = row;

			for (var i = 0; i < GridSizeX; i++)
			{
				var node = new WcfNode();

				node.IsCollapsed = false;

				node.Options = new List<WcfTile>();

				foreach (var origTile in Tiles)
				{
					node.Options.Add(origTile);
				}

				row[i] = node;
			}
		}

		return grid;
	}

	private void RenderGrid(WcfNode[][] grid)
	{
		for(int i = 0; i < Container.GetChildCount(); i++)
		{
			Container.GetChild(i).QueueFree();
		}

		for (var x = 0; x < GridSizeY; x++)
		{
			for (var i = 0; i < GridSizeX; i++)
			{
				var node = grid[x][i];

				if (!node.IsCollapsed)
				{
					var t = Tiles[0].Tile.Instantiate<TileDebug>();
					Container.AddChild(t);

					t.ValidOptions = node.Options.Count;

					t.GlobalPosition = new Vector3(i, 0, x);
				}
				else
				{
					var tile = node.Options[0].Tile.Instantiate<Node3D>();
					Container.AddChild(tile);

					tile.GlobalPosition = new Vector3(i, 0, x);
				}


			}
		}
	}
}
