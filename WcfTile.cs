using Godot;

[GlobalClass]
public partial class WcfTile : Resource
{
    [Export] public PackedScene Tile;

    [Export] public PackedScene[] Top;

    [Export] public PackedScene[] Right;

    [Export] public PackedScene[] Down;

    [Export] public PackedScene[] Left;
}
