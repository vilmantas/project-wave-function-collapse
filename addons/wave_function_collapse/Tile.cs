using Godot;
using System;

namespace  WaveFunctionCollapse.Godot.Plugin;

[Tool]
public partial class Tile : Control
{
    [Export] public LineEdit txt_TopConnectors;
    [Export] public LineEdit txt_RightConnectors;
    [Export] public LineEdit txt_BottomConnectors;
    [Export] public LineEdit txt_LeftConnectors;

    [Export] public Label lbl_Title;

    public void Initialize(string fileName, GodotTile tile)
    {
        lbl_Title.Text = fileName;

        txt_TopConnectors.Text = tile.TopConnectors;
        txt_RightConnectors.Text = tile.RightConnectors;
        txt_BottomConnectors.Text = tile.BottomConnectors;
        txt_LeftConnectors.Text = tile.LeftConnectors;
    }
}
