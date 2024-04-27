using Godot;
using System;

namespace WaveFunctionCollapse.Godot.Plugin.MapPreset;

[Tool]
public partial class TilePreview : HBoxContainer
{
    [Export] public Label lbl_Id;
    [Export] Label lbl_TileName;
    [Export] public TextureRect img_Preview;
    [Export] Button btn_Select;

    public Action<TilePreview> OnTilePressed;

    public override void _Ready()
    {
        btn_Select.Pressed += () => OnTilePressed(this);
    }

    public void Initialize(string tileName, string id, string snapshotPath, int imageRotation)
    {
        lbl_TileName.Text = tileName;
        lbl_Id.Text = id;

        if (string.IsNullOrEmpty(snapshotPath)) return;

        img_Preview.Texture = ResourceLoader.Load<Texture2D>(snapshotPath);

        if (imageRotation == 0) return;

        img_Preview.RotationDegrees = imageRotation;

    }

    public override void _Process(double delta)
    {
        img_Preview.PivotOffset = img_Preview.Size / 2;
    }
}
