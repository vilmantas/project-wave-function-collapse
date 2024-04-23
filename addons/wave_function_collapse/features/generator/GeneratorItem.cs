using Godot;
using System;
using System.Collections.Generic;

namespace WaveFunctionCollapse.Godot.Plugin;

[Tool]
public partial class GeneratorItem : Control
{
	private Generator Parent;

	[Export] public PackedScene Snapshotter;

	[Export] public Label lbl_FileName;
	[Export] public TextureRect img_preview;

	[Export] public LineEdit txt_TopConnectors;
	[Export] public LineEdit txt_RightConnectors;
	[Export] public LineEdit txt_BottomConnectors;
	[Export] public LineEdit txt_LeftConnectors;

	[Export] public CheckButton btn_GenerateRotations;
	[Export] public CheckButton btn_IsSymmetric;
	[Export] public CheckButton btn_OverrideExisting;

	[Export] public LineEdit txt_ResourceName;

	[Export] public Button btn_Generate;

	private string m_ImportFullPath;

	public override void _Ready()
	{
		btn_Generate.Pressed += GenerateResources;
	}

	public void Initialize(string fileName, string importFullPath, Generator parent, bool generateSnapshot)
	{
		Parent = parent;

		m_ImportFullPath = importFullPath;

		lbl_FileName.Text = fileName;

		txt_ResourceName.Text = fileName.Split(".")[0];

		if (!generateSnapshot) return;

		var tileInstance = ResourceLoader.Load<PackedScene>(importFullPath).Instantiate<Node3D>();

		AddChild(tileInstance);

		var snapshotterInstance = Snapshotter.Instantiate<TileSnapshotter>();

		tileInstance.AddChild(snapshotterInstance);

		snapshotterInstance.Initialize("res://addons/wave_function_collapse/test/" + fileName +
		                               "-snapshot.png");

		GetTree().CreateTimer(1f).Timeout += () =>
		{
			img_preview.Texture = ResourceLoader.Load<Texture2D>("res://addons/wave_function_collapse/test/" + fileName + "-snapshot.png");
		};
	}

	private void GenerateResources()
	{
		var tile = new GodotTile();

		tile.Prefab = ResourceLoader.Load<PackedScene>(m_ImportFullPath);

		tile.TopConnectors = txt_TopConnectors.Text;
		tile.RightConnectors = txt_RightConnectors.Text;
		tile.BottomConnectors = txt_BottomConnectors.Text;
		tile.LeftConnectors = txt_LeftConnectors.Text;

		SaveTile(tile);

		if (btn_GenerateRotations.ButtonPressed)
		{
			var rotations = RotationsFor(tile);

			foreach (var rotation in rotations)
			{
				SaveTile(rotation);
			}
		}
	}

	public void SaveTile(GodotTile tile)
	{
		var fileName = tile.RotationY == 0 ? txt_ResourceName.Text : $"{txt_ResourceName.Text}-{tile.RotationY}";

		var filePath = Parent.txt_ExportDestination.Text + fileName + ".tres";

		ResourceSaver.Save(tile, filePath);
	}

	public static IEnumerable<GodotTile> RotationsFor(GodotTile tile)
	{
		var result = new List<GodotTile>();

		var connections = new List<string>()
		{
			tile.TopConnectors, tile.RightConnectors, tile.BottomConnectors, tile.LeftConnectors
		};

		for (int i = 1; i < 4; i++)
		{
			var lastElement = connections[^1];

			connections.RemoveAt(connections.Count - 1);
			connections.Insert(0, lastElement);

			var rotatedTile = tile.Copy();

			rotatedTile.TopConnectors = connections[0];
			rotatedTile.RightConnectors = connections[1];
			rotatedTile.BottomConnectors = connections[2];
			rotatedTile.LeftConnectors = connections[3];
			rotatedTile.RotationY = i * 90;

			result.Add(rotatedTile);
		}

		return result;
	}
}
