using Godot;
using System;

public partial class TileDebug : Node3D
{
	[Export] public Label3D Label;
	public int ValidOptions = 0;
	public int X;
	public int Y;

	public override void _Process(double delta)
	{
		Label.Text = $"{ValidOptions}-{X}:{Y}";
		Label.GlobalBasis = GetViewport().GetCamera3D().GlobalBasis;
	}
}
