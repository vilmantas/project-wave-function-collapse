using System;
using WaveFunctionCollapse.Collapsing;
using WaveFunctionCollapse.Constraints;

namespace WaveFunctionCollapse;

public static class Algorithm
{
    public static Random Random { get; set; } = new Random();

    public static CellCollapseStrategy CollapseStrategy { get; set; } = CellCollapseStrategy.Weight;

    public enum CellCollapseStrategy
    {
        Random,
        Weight,
    }

    public static bool Run(Grid grid)
    {
        while (!grid.IsCollapsed)
        {
            var candidates = grid.LowestEntropyCells();

            var cell = candidates[Random.Next(candidates.Length)];

            if (cell.Entropy == 0) return false;

            Collapse(cell);

            SimpleNeighbourConnectorStrategy.Process(cell);
        }

        return true;
    }

    public static bool Step(Grid grid)
    {
        if (grid.IsCollapsed) return false;

        LimitStrategy.Process(grid);

        var candidates = grid.LowestEntropyCells();

        var cell = candidates[Random.Next(candidates.Length)];

        cell.Random = Random;

        if (cell.Entropy == 0) return false;

        Collapse(cell);

        SimpleNeighbourConnectorStrategy.Process(cell);

        return true;
    }

    private static void Collapse(Cell cell)
    {
        switch (CollapseStrategy)
        {
            case CellCollapseStrategy.Weight:
                WeightCollapseStrategy.Collapse(cell);
                break;
            case CellCollapseStrategy.Random:
                RandomCollapseStrategy.Collapse(cell);
                break;
        }
    }
}