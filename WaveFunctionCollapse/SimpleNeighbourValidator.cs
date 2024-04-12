using System.Collections.Generic;
using System.Linq;
using Godot;

namespace WaveFunctionCollapse;

public static class SimpleNeighbourValidator
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

        var validOptions = new List<string>();

        if (cell.Up != null)
        {
            validOptions = cell.Up.Options.Select(x => x.BottomConnectors).ToList();

            cell.Options.RemoveAll(x => !validOptions.Contains(x.TopConnectors));
        }

        if (cell.Down != null)
        {
            validOptions = cell.Down.Options.Select(x => x.TopConnectors).ToList();

            cell.Options.RemoveAll(x => !validOptions.Contains(x.BottomConnectors));
        }

        if (cell.Left != null)
        {
            validOptions = cell.Left.Options.Select(x => x.RightConnectors).ToList();

            cell.Options.RemoveAll(x => !validOptions.Contains(x.LeftConnectors));
        }

        if (cell.Right != null)
        {
            validOptions = cell.Right.Options.Select(x => x.LeftConnectors).ToList();

            cell.Options.RemoveAll(x => !validOptions.Contains(x.RightConnectors));
        }
    }
}