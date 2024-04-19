namespace WaveFunctionCollapse;

public static class Algorithm
{
    public static bool Run(Grid grid)
    {
        var random = new System.Random();

        BorderStrategy.Process(grid);

        while (!grid.IsCollapsed)
        {
            var candidates = grid.LowestEntropyCells();

            var cell = candidates[random.Next(candidates.Length)];

            if (cell.Entropy == 0) return false;

            cell.Collapse();

            SimpleNeighbourValidator.Process(cell);

            LimitStrategy.Process(grid);
        }

        return true;
    }

    public static bool Step(Grid grid)
    {
        if (grid.IsCollapsed) return false;

        var random = new System.Random();

        var candidates = grid.LowestEntropyCells();

        var cell = candidates[random.Next(candidates.Length)];

        cell.Random = random;

        if (cell.Entropy == 0) return false;

        cell.Collapse();

        SimpleNeighbourValidator.Process(cell);

        LimitStrategy.Process(grid);

        return true;
    }
}