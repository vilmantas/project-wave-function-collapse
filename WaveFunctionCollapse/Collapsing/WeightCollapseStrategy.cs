using System.Collections.Generic;
using System.Linq;
using Godot;

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

            GD.Print($"Cell {cell.X},{cell.Y} collapsed to {option.Name} with weight {option.Weight} and a roll of {roll} from {tickets} tickets and {cell.Options.Count} total options.");

            cell.Options = new List<Tile> { option };
            break;
        }
    }
}