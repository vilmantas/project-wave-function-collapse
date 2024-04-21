using Godot;
using System;
using System.Linq;
using WaveFunctionCollapse.Godot;

public partial class debug_scene : Node3D
{
	[Export] public GodotTiles Configuration;

	public GodotTile[] Tiles => Configuration.Tiles;

	public Aabb Bounds;

	public int currentIndexX = 0;
	public int currentIndexY = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var instance = Configuration.Tiles.First().Prefab.Instantiate<Node3D>();

		Bounds = instance.GetNode<MeshInstance3D>("model").GetAabb();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Input(InputEvent @event)
	{

		if (Input.IsActionJustPressed("clear"))
		{
			var random = new Random();

			var instance = Tiles[random.Next(Tiles.Length)].Prefab.Instantiate<Node3D>();

			AddChild(instance);

			instance.GlobalPosition = new Vector3(currentIndexX * Bounds.Size.X, 0, -currentIndexY * Bounds.Size.Y);
			instance.RotationDegrees = new Vector3(0, random.Next(0, 4) * 90, 0);

			currentIndexX++;

			if (currentIndexX > 4)
			{
				currentIndexX = 0;
				currentIndexY++;
			}
		}
	}
}
