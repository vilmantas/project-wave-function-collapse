using Godot;
using System;
using System.IO;
using FileAccess = Godot.FileAccess;

namespace  WaveFunctionCollapse.Godot.Plugin;

[Tool]
public partial class Tile : Control
{
    [Export] public LineEdit txt_TopConnectors;
    [Export] public LineEdit txt_RightConnectors;
    [Export] public LineEdit txt_BottomConnectors;
    [Export] public LineEdit txt_LeftConnectors;

    [Export] public Label lbl_Title;

    [Export] public TextureRect img_Preview;

    public void Initialize(string tileName, GodotTile tile)
    {
        lbl_Title.Text = tileName;

        txt_TopConnectors.Text = tile.TopConnectors;
        txt_RightConnectors.Text = tile.RightConnectors;
        txt_BottomConnectors.Text = tile.BottomConnectors;
        txt_LeftConnectors.Text = tile.LeftConnectors;

        var previewPath = Path.Combine(Main.SNAPSHOTS_PATH,
            $"{tileName.Split("-")[0]}-snapshot.png");

        if (!FileAccess.FileExists(previewPath)) return;

        img_Preview.Texture =
            ResourceLoader.Load<Texture2D>(previewPath);

        if (tile.RotationY == 0) return;

        img_Preview.PivotOffset = img_Preview.Size / 2;

        img_Preview.RotationDegrees = tile.RotationY;
    }

    public override void _Process(double delta)
    {
        img_Preview.PivotOffset = img_Preview.Size / 2;
    }

    public void SetEditingEnabled(bool enabled)
    {
        txt_TopConnectors.Editable = enabled;
        txt_RightConnectors.Editable = enabled;
        txt_BottomConnectors.Editable = enabled;
        txt_LeftConnectors.Editable = enabled;
    }
}
