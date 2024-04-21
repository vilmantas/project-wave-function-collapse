using Godot;
using System;
using WaveFunctionCollapse;

public partial class TileDebug : Node3D
{
	[Export] public Area3D Area;

	[Export] public Label3D CoordinatesLabel;

	public Cell Cell;

	public static bool Debug = false;

	public override void _Ready()
	{
		Area.MouseEntered += () => Main.ShowCellDebug(Cell);
		Area.MouseExited += () => Main.ClearCellDebug();
	}

	public void InitializeFromCell(Cell cell)
	{
		Visible = Debug;
		Cell = cell;

		CoordinatesLabel.Text = $"{cell.X}, {cell.Y}";
	}

	public override void _Process(double delta)
	{
		Visible = Debug;

		CoordinatesLabel.GlobalBasis = GetViewport().GetCamera3D().GlobalBasis;
	}
}
