using Godot;
using WaveFunctionCollapse;

namespace WaveFunctionCollapse.Godot;

[GlobalClass]
public partial class GodotTiles : Resource
{
    [Export] public GodotTile[] Tiles;
}