using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using WaveFunctionCollapse.Godot;

namespace WaveFunctionCollapse.Godot.Plugin;

[Tool]
public partial class Main : Control
{
	[Export] public Control CurrentTilesContainer;

	[Export] public PackedScene TileScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var tilePath = "res://tile_resources/debug";

		var tiles = DirAccess.GetFilesAt(tilePath);

		var allTiles = new List<GodotTile>();

		foreach (var tile in tiles)
		{
			var tileResource = ResourceLoader.Load<GodotTile>(Path.Combine(tilePath, tile));

			allTiles.Add(tileResource);

			var instance = TileScene.Instantiate<Tile>();

			instance.Initialize(tile, tileResource);

			CurrentTilesContainer.AddChild(instance);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
