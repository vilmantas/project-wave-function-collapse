using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using FileAccess = Godot.FileAccess;

namespace WaveFunctionCollapse.Godot.Plugin;

[Tool]
public partial class GeneratorItem : Control
{
	private Generator Parent;

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

	public void Initialize(string fileName, string importFullPath, Generator parent)
	{
		Parent = parent;

		m_ImportFullPath = importFullPath;

		lbl_FileName.Text = fileName;

		txt_ResourceName.Text = fileName;

		LoadSnapshot();
	}

	public void LoadSnapshot()
	{
		var snapshotPath = Path.Combine(Main.SNAPSHOTS_PATH, lbl_FileName.Text + "-snapshot.png");

		if (!FileAccess.FileExists(snapshotPath)) return;

		img_preview.Texture = ResourceLoader.Load<Texture2D>(snapshotPath);
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
			var rotations = RotationsFor(tile, btn_IsSymmetric.ButtonPressed);

			foreach (var rotation in rotations)
			{
				SaveTile(rotation);
			}
		}
	}

	public void SaveTile(GodotTile tile)
	{
		var fileName = $"{txt_ResourceName.Text}-{tile.RotationY}";

		var filePath = Parent.txt_ExportDestination.Text + fileName + ".tres";

		ResourceSaver.Save(tile, filePath);
	}

	public static IEnumerable<GodotTile> RotationsFor(GodotTile tile, bool isSymmetric)
	{
		var result = new List<GodotTile>();

		var connections = new List<string>()
		{
			tile.TopConnectors, tile.RightConnectors, tile.BottomConnectors, tile.LeftConnectors
		};

		var iterations = isSymmetric ? 2 : 4;

		for (int i = 1; i < iterations; i++)
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
