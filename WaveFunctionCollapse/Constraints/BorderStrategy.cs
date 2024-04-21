using System.Collections.Generic;
using System.Linq;
using Godot;

namespace WaveFunctionCollapse;

public static class BorderStrategy
{
    public static void Process(Grid grid)
    {
        foreach (var cell in grid.Borders())
        {
            var newOptions = new List<Tile>();

            foreach (var option in cell.Options)
            {
                if (!option.IsBorder) continue;

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

        foreach (var cell in grid.Corners())
        {
            switch (cell.Up)
            {
                case null when cell.Left == null:
                    cell.Options = cell.Options.Where(x => x.TopConnectors == "EMPTY" && x.LeftConnectors == "EMPTY").ToList();
                    continue;
                case null when cell.Right == null:
                    cell.Options = cell.Options.Where(x => x.TopConnectors == "EMPTY" && x.RightConnectors == "EMPTY").ToList();
                    continue;
            }

            switch (cell.Down)
            {
                case null when cell.Left == null:
                    cell.Options = cell.Options.Where(x => x.BottomConnectors == "EMPTY" && x.LeftConnectors == "EMPTY").ToList();
                    continue;
                case null when cell.Right == null:
                    cell.Options = cell.Options.Where(x => x.BottomConnectors == "EMPTY" && x.RightConnectors == "EMPTY").ToList();
                    continue;
            }
        }
    }
}