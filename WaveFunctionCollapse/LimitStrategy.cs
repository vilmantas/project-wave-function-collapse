using System.Linq;

namespace WaveFunctionCollapse;

public static class LimitStrategy
{
    public static void Process(Grid grid)
    {
        foreach (var tile in grid.AvailableTiles.Where(x => x.LimitEnabled))
        {
            var count = grid.CollapsedCells().Count(x => x.Options[0] == tile);

            if (count < tile.Limit) continue;

            foreach (var cell in grid.ProcessableCells())
            {
                cell.Options.RemoveAll(x => x == tile);
            }
        }
    }
}