using System.Linq;

namespace WaveFunctionCollapse.Constraints;

public static class LimitStrategy
{
    public static void Process(Grid grid)
    {
        foreach (var grouping in grid.AvailableTiles.Where(x => x.LimitEnabled).GroupBy(x => x.Name))
        {
            var tile = grouping.First();

            var count = grid.CollapsedCells().Count(x => x.Options[0].Name == tile.Name);

            if (count < tile.Limit) continue;

            foreach (var cell in grid.ProcessableCells())
            {
                cell.Options.RemoveAll(x => x.Name == tile.Name);
            }
        }
    }
}