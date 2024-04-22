using System.Collections.Generic;
using System.Linq;

namespace WaveFunctionCollapse.Collapsing;

public static class WeightCollapseStrategy
{
    public static void Collapse(Cell cell)
    {
        if (cell.IsCollapsed) return;

        cell.IsCollapsed = true;

        var tickets = cell.Options.Sum(x => x.Weight);

        var roll = cell.Random.Next(1, tickets + 1);

        var sum = 0;

        foreach (var option in cell.Options)
        {
            sum += option.Weight;

            if (roll > sum) continue;

            cell.Options = new List<Tile> { option };
            break;
        }
    }
}