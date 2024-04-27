using Godot;
using System;

namespace WaveFunctionCollapse.Godot.Plugin.MapPreset;

[Tool]
public partial class MapTile : Control
{
    [Export] public Label lbl_Coords;

    [Export] public TextureRect img_Preview;

    [Export] public Button btn_Tile;

    private int _rotation;

    public int Id = -1;

    public Action<MapTile> OnTilePressed;

    public void Initialize(string coords)
    {
        lbl_Coords.Text = coords;

        btn_Tile.Pressed += () => OnTilePressed(this);
    }

    public override void _Process(double delta)
    {
        img_Preview.PivotOffset = img_Preview.Size / 2;
    }

    public void SetTile(Texture2D texture, int rotation, int id)
    {
        img_Preview.Texture = texture;
        img_Preview.RotationDegrees = rotation;

        _rotation = rotation;

        lbl_Coords.Text = String.Empty;

        Id = id;
    }
}
