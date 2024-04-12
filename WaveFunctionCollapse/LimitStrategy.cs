using System.Linq;

namespace WaveFunctionCollapse;

public static class LimitStrategy
{
    public static void Process(Grid grid)
    {
        foreach (var grouping in grid.AvailableTiles.GroupBy(x => x.Name).Where(x => x.First().LimitEnabled))
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