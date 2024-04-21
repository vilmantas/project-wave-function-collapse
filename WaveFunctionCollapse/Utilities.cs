using System.Collections.Generic;
using System.Linq;

namespace WaveFunctionCollapse;

public static class Utilities
{
    public static IEnumerable<Tile> GenerateRotations(IEnumerable<Tile> originalTiles)
    {
        var rotatedTiles = new List<Tile>();

        foreach (var tile in originalTiles)
        {
            rotatedTiles.AddRange(RotationsFor(tile));
        }

        return rotatedTiles;
    }

    public static IEnumerable<Tile> RotationsFor(Tile tile)
    {
        var result = new List<Tile>();

        var connections = tile.Connectors.ToList();

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

        return result;
    }
}