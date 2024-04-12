using System.Collections.Generic;
using System.Linq;

namespace WaveFunctionCollapse;

public class Grid
{
    public readonly Tile[] AvailableTiles;

    public readonly Cell[,] Cells;

    public (int x, int y) DimensionsFromIndex(int i) => (i / Cells.GetLength(0), i % Cells.GetLength(0));
    public int IndexFromDimensions(int x, int y) => x * Cells.GetLength(0) + y;

    public bool IsCollapsed => Cells.Cast<Cell>().All(x => x.IsCollapsed);

    public Cell this[int x, int y] => Cells[x, y];

    public Cell this[int i]
     {
         get
         {
             var x = i / Cells.GetLength(0);
             var y = i % Cells.GetLength(0);
             return Cells[x,y];
         }
         set
         {
             var x = i / Cells.GetLength(0);
             var y = i % Cells.GetLength(0);
             Cells[x,y] = value;
         }
    }

    public Grid(int x, int y, Tile[] tiles)
    {
        Cells = new Cell[x, y];

        AvailableTiles = tiles;

        InitializeEmptyGrid();
    }

    public Cell[] LowestEntropyNodes()
    {
        var cells = ProcessableCells();

        var lowestEntropy = cells.Min(x => x.Entropy);

        return cells.Where(x => x.Entropy == lowestEntropy).ToArray();
    }

    public Cell[] ProcessableCells() => Cells.Cast<Cell>().Where(x => !x.IsCollapsed).ToArray();

    private void InitializeEmptyGrid()
    {
        for (int i = 0; i < Cells.Length; i++)
        {
            var dimension = DimensionsFromIndex(i);

            var cell = new Cell
            {
                X = dimension.x,
                Y = dimension.y,
                Options = new List<Tile>(AvailableTiles),
            };

            this[i] = cell;
        }

        for (int x = 0; x < Cells.GetLength(0); x++)
        {
            for (int y = 0; y < Cells.GetLength(1); y++)
            {
                var cell = this[x, y];

                cell.Down = y > 0 ? Cells[x, y - 1] : null;
                cell.Right = x < Cells.GetLength(0) - 1 ? Cells[x + 1, y] : null;
                cell.Up = y < Cells.GetLength(1) - 1 ? Cells[x, y + 1] : null;
                cell.Left = x > 0 ? Cells[x - 1, y] : null;
            }
        }
    }
}