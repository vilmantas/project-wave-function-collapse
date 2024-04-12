namespace WaveFunctionCollapse;

public static class Algorithm
{
    public static bool Run(Grid grid)
    {
        var random = new System.Random();

        while (!grid.IsCollapsed)
        {
            var candidates = grid.LowestEntropyCells();

            var cell = candidates[random.Next(candidates.Length)];

            if (cell.Entropy == 0) return false;

            cell.Collapse();

            SimpleNeighbourValidator.Process(cell);
        }

        return true;
    }
}