using System.Collections.Generic;
using System.Linq;
using Godot;

namespace WaveFunctionCollapse;

public static class BorderStrategy
{
    public static void Process(Grid grid)
    {
        var borderTiles = grid.AvailableTiles.Where(x =>
            x.Positioning is PositionMode.Border or PositionMode.All).ToArray();

        var centerTiles = grid.AvailableTiles.Where(x =>
            x.Positioning is PositionMode.Center or PositionMode.All).ToArray();

        foreach (var cell in grid.Cells)
        {
            var neighbours = new[] { cell.Up, cell.Down, cell.Left, cell.Right };

            var isBorder = neighbours.Any(x => x == null);

            cell.Options = isBorder ?
                cell.Options.Where(x => borderTiles.Contains(x)).ToList() :
                cell.Options.Where(x => centerTiles.Contains(x)).ToList();

            if (!isBorder) continue;

            var isCorner = neighbours.Count(x => x == null) == 2;

            if (isCorner)
            {
                cell.Options = cell.Options
                    .Where(x => x.Connectors.All(x => x != "EMPTY"))
                    .ToList();

                continue;
            }

            var newOptions = new List<Tile>();

            foreach (var option in cell.Options)
            {
                var connectors = new List<string>()
                {
                    option.TopConnectors, option.RightConnectors,
                    option.BottomConnectors, option.LeftConnectors
                };

                if (connectors.All(x => x != "EMPTY"))
                {
                    newOptions.Add(option);
                    continue;
                }

                if (cell.Up == null && option.TopConnectors == "EMPTY")
                {
                    newOptions.Add(option);
                    continue;
                }

                if (cell.Down == null && option.BottomConnectors == "EMPTY")
                {
                    newOptions.Add(option);
                    continue;
                }

                if (cell.Left == null && option.LeftConnectors == "EMPTY")
                {
                    newOptions.Add(option);
                    continue;
                }

                if (cell.Right == null && option.RightConnectors == "EMPTY")
                {
                    newOptions.Add(option);
                }
            }

            cell.Options = newOptions;
        }
    }
}