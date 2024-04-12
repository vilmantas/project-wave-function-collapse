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

	public bool Debug = false;

	public override void _Process(double delta)
	{
		if (!Debug)
		{
			Label.Text = "";
			return;
		};

		Label.Text = $"{ValidOptions} ({X},{Y})";
		Label.GlobalBasis = GetViewport().GetCamera3D().GlobalBasis;
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsKeyPressed(Key.F))
		{
			Debug = !Debug;
		}
	}
}
