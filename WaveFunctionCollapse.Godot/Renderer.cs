using System.Linq;
using Godot;

namespace WaveFunctionCollapse.Godot;

public static class Renderer
{
    public static void RenderGrid(GodotTile[] tiles, Grid grid, Node3D container, Vector3 bounds)
    {
        foreach (var cell in grid.Cells)
        {
            if (cell.IsBroken) continue;

            var tile = SelectCellTile(cell, tiles).Prefab
                .Instantiate<Node3D>();

            container.AddChild(tile);

            tile.GlobalPosition = new Vector3(cell.X * bounds.X, 0, -cell.Y * bounds.Z);
            tile.RotationDegrees = new Vector3(0, -cell.Options[0].RotationY, 0);

            if (Main.DebugPrefab == null) continue;

            var debug = Main.DebugPrefab.Instantiate<TileDebug>();

            debug.InitializeFromCell(cell);

            container.AddChild(debug);

            debug.GlobalPosition = new Vector3(tile.GlobalPosition.X, tile.GlobalPosition.Y + 1f, tile.GlobalPosition.Z);
        }
    }

    private static GodotTile SelectCellTile(Cell cell, GodotTile[] tiles)
    {
        return !cell.IsCollapsed ? tiles[0] : tiles.First(x => x.ResourcePath == cell.Options[0].Name);
    }
}