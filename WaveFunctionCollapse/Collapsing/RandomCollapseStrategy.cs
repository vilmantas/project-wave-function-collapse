using System.Collections.Generic;

namespace WaveFunctionCollapse.Collapsing;

public static class RandomCollapseStrategy
{
    public static void Collapse(Cell cell)
    {
        if (cell.IsCollapsed) return;

        cell.IsCollapsed = true;

        var option = cell.Options[cell.Random.Next(cell.Options.Count)];

        cell.Options = new List<Tile> { option };
    }
}