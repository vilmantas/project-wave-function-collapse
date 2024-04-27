using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using WaveFunctionCollapse.Godot;
using WaveFunctionCollapse.Godot.Plugin;

namespace WaveFunctionCollapse.Godot.Plugin;

[Tool]
public partial class Importer : Node
{
	[Export] public Control ctn_Tiles;

	[Export] public PackedScene TileScene;

	[Export] public LineEdit txt_ImportLocation;

	[Export] public Button btn_Import;

	public List<GodotTile> AllTiles;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		btn_Import.Pressed += LoadTiles;
	}

	public void LoadTiles()
	{
		var tilePath = txt_ImportLocation.Text;

		var tiles = DirAccess.GetFilesAt(tilePath);

		var allTiles = new List<GodotTile>();

		foreach (var tileFile in tiles)
		{
			var tileResource = ResourceLoader.Load<GodotTile>(Path.Combine(tilePath, tileFile));

			var tileName = tileFile.Split('.')[0];

			allTiles.Add(tileResource);

			var instance = TileScene.Instantiate<Tile>();

			instance.Initialize(tileName, tileResource);

			instance.SetEditingEnabled(false);

			ctn_Tiles.AddChild(instance);
		}

		AllTiles = allTiles;
	}
}
