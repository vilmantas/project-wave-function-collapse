using Godot;
using System;

public partial class TileDebug : Node3D
{
	[Export] public Label3D Label;
	public int ValidOptions = 0;
	public int X;
	public int Y;
	public bool Up;
	public bool Right;
	public bool Down;
	public bool Left;

	public static bool Debug = false;

	public override void _Ready()
	{
		Label.Text = "";
	}

	public override void _Process(double delta)
	{
		if (!TileDebug.Debug)
		{
			Label.Text = "";
			return;
		};

		Label.Text = $"{ValidOptions} ({X},{Y})";
		Label.GlobalBasis = GetViewport().GetCamera3D().GlobalBasis;
	}
}
