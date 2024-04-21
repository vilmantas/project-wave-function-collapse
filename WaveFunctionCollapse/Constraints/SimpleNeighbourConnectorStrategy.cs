using System.Collections.Generic;
using System.Linq;
using Godot;

namespace WaveFunctionCollapse;

public static class SimpleNeighbourConnectorStrategy
{
    public static void Process(Cell cell)
    {
        ReduceOptionsByNeighbours(cell.Up);
        ReduceOptionsByNeighbours(cell.Down);
        ReduceOptionsByNeighbours(cell.Right);
        ReduceOptionsByNeighbours(cell.Left);
    }

    private static void ReduceOptionsByNeighbours(Cell cell)
    {
        if (cell == null) return;

        List<string> validOptions;

        if (cell.Up != null)
        {
            validOptions = cell.Up.Options.Select(x => new string(x.BottomConnectors.ToCharArray().Reverse().ToArray())).ToList();

            cell.Options.RemoveAll(x => !validOptions.Contains(x.TopConnectors) || x.TopConnectors == "EMPTY");
        }

        if (cell.Down != null)
        {
            validOptions = cell.Down.Options.Select(x => new string(x.TopConnectors.ToCharArray().Reverse().ToArray())).ToList();

            cell.Options.RemoveAll(x => !validOptions.Contains(x.BottomConnectors) || x.BottomConnectors == "EMPTY");
        }

        if (cell.Left != null)
        {
            validOptions = cell.Left.Options.Select(x => new string(x.RightConnectors.ToCharArray().Reverse().ToArray())).ToList();

            cell.Options.RemoveAll(x => !validOptions.Contains(x.LeftConnectors) || x.LeftConnectors == "EMPTY");
        }

        if (cell.Right != null)
        {
            validOptions = cell.Right.Options.Select(x => new string(x.LeftConnectors.ToCharArray().Reverse().ToArray())).ToList();

            cell.Options.RemoveAll(x => !validOptions.Contains(x.RightConnectors) || x.RightConnectors == "EMPTY");
        }
    }
}