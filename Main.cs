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

    public Grid Grid;

    public override void _Ready()
    {
	    Tiles = Configuration.Tiles;

	    var newTiles = Tiles.ToList();

	    newTiles.AddRange(Utilities.GenerateRotations(Tiles));

	    Tiles = newTiles.ToArray();
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
		    Grid = new Grid(GridSizeX, GridSizeY,
			    Tiles.Select(x => x.ToTile()).ToArray());

		    BorderStrategy.Process(Grid);
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

	    if (Input.IsKeyPressed(Key.F))
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

		foreach (var node in grid.Cells)
		{
			if (!node.IsCollapsed)
			{
				if (node.IsBroken) continue;

				var tile = Tiles[0].Prefab.Instantiate<Node3D>();

				if (tile is TileDebug debugTile)
				{
					debugTile.ValidOptions = node.Entropy;
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

	private GodotTile FindTile(string name) => Tiles.First(x => x.ResourcePath == name);
}
