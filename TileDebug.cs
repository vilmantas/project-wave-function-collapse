using Godot;
using System;

public partial class TileDebug : Node3D
{
	[Export] public Label3D Label;
	public int ValidOptions = 0;

	public override void _Process(double delta)
	{
		Label.Text = ValidOptions.ToString();
		Label.GlobalBasis = GetViewport().GetCamera3D().GlobalBasis;
	}
}
