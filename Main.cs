using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using WaveFunctionCollapse;
using WaveFunctionCollapse.Godot;
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

	    var newTiles = Tiles.ToList();

	    newTiles.AddRange(Utilities.GenerateRotations(Tiles));

	    Tiles = newTiles.ToArray();
    }

    public override void _Process(double delta)
    {
	    if (Paused) return;
    }

    public override void _Input(InputEvent @event)
    {
	    if (Input.IsActionJustPressed("generate"))
	    {
		    Grid grid = new Grid(GridSizeX, GridSizeY,
			    Tiles.Select(x => x.ToTile()).ToArray());

		    while (!Algorithm.Run(grid)) ;

		    RenderGrid(grid);
	    }

	    if (Input.IsActionJustPressed("pause"))
	    {
		    Paused = !Paused;
	    }

	    if (Input.IsActionJustPressed("clear"))
	    {
		    ClearContainer();
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
				tile.RotationDegrees = new Vector3(0, -node.Options[0].Rotation, 0);
			}
		}
	}

	private GodotTile FindTile(string name) => Tiles.First(x => x.Prefab.ResourcePath == name);
}
