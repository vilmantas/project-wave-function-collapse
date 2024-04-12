using System.Collections.Generic;
using System.Linq;

namespace WaveFunctionCollapse.Godot;

public static class Utilities
{
    public static GodotTile[] GenerateRotations(GodotTile[] originalTiles)
    {
        var rotatedTiles = new List<GodotTile>();

        foreach (var tile in originalTiles.Where(x => x.GenerateRotations))
        {
            rotatedTiles.AddRange(RotationsFor(tile));
        }

        return rotatedTiles.ToArray();
    }

    public static GodotTile[] RotationsFor(GodotTile tile)
    {
        var result = new List<GodotTile>();

        var connections = new List<string>()
        {
            tile.TopConnectors, tile.RightConnectors, tile.BottomConnectors, tile.LeftConnectors
        };

        for (int i = 1; i < 4; i++)
        {
            var lastElement = connections[^1];
            connections.RemoveAt(connections.Count - 1);
            connections.Insert(0, lastElement);

            var rotatedTile = tile.Copy();

            rotatedTile.TopConnectors = connections[0];
            rotatedTile.RightConnectors = connections[1];
            rotatedTile.BottomConnectors = connections[2];
            rotatedTile.LeftConnectors = connections[3];
            rotatedTile.RotationY = i * 90;

            result.Add(rotatedTile);
        }

        return result.ToArray();
    }
}