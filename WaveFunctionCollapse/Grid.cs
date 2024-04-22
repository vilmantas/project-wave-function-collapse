using System.Collections.Generic;
using System.Linq;

namespace WaveFunctionCollapse;

public class Grid
{
    public readonly Tile[] AvailableTiles;

    public readonly Cell[,] Cells;

    public readonly int GridSizeX = -1;

    public readonly int GridSizeY = -1;

    public (int x, int y) DimensionsFromIndex(int i) => (i % Cells.GetLength(1), i / Cells.GetLength(1));
    public int IndexFromDimensions(int x, int y) => y * Cells.GetLength(1) + x;

    public bool IsCollapsed => Cells.Cast<Cell>().All(x => x.IsCollapsed);

    public Cell this[int x, int y] => Cells[y, x];

    public Cell this[int i]
     {
         get
         {
             var x = i % Cells.GetLength(1);
             var y = i / Cells.GetLength(1);
             return Cells[x,y];
         }
         set
         {
             var x = i % Cells.GetLength(1);
             var y = i / Cells.GetLength(1);
             Cells[y,x] = value;
         }
    }

    public Grid(int x, int y, Tile[] tiles)
    {
        GridSizeX = x;

        GridSizeY = y;

        Cells = new Cell[y, x];

        AvailableTiles = tiles;

        InitializeEmptyGrid();
    }

    public Grid(int x, int y, Tile[] tiles, int[,] preset)
    {
        GridSizeX = x;

        GridSizeY = y;

        Cells = new Cell[y, x];

        AvailableTiles = tiles;

        InitializeEmptyGrid();

        MapPreset(preset);
    }

    private void MapPreset(int[,] preset)
    {
        for (int x = 0; x < Cells.GetLength(1); x++)
        {
            for (int y = 0; y < Cells.GetLength(0); y++)
            {
                if (preset[y, x] == -1) continue;

                var cell = this[x, y];

                cell.Options = new List<Tile> { AvailableTiles[preset[y, x]] };
                cell.IsCollapsed = true;
            }
        }
    }

    public Cell[] LowestEntropyCells()
    {
        var cells = ProcessableCells();

        var lowestEntropy = cells.Min(x => x.Entropy);

        return cells.Where(x => x.Entropy == lowestEntropy).ToArray();
    }

    public Cell[] ProcessableCells() => Cells.Cast<Cell>().Where(x => !x.IsCollapsed).ToArray();

    public Cell[] CollapsedCells() => Cells.Cast<Cell>().Where(x => x.IsCollapsed).ToArray();

    public Cell[] Corners() => new Cell[] { this[0, 0], this[0, Cells.GetLength(0) - 1], this[Cells.GetLength(1) - 1, 0], this[Cells.GetLength(1) - 1, Cells.GetLength(0) - 1] };

    public Cell[] Borders() => Cells.Cast<Cell>().Where(x => x.Neighbours.Count(y => y == null) == 1).ToArray();

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

        for (int x = 0; x < Cells.GetLength(1); x++)
        {
            for (int y = 0; y < Cells.GetLength(0); y++)
            {
                var cell = this[x, y];

                cell.Down = y > 0 ? this[x, y - 1] : null;
                cell.Right = x < Cells.GetLength(1) - 1 ? this[x + 1, y] : null;
                cell.Up = y < Cells.GetLength(0) - 1 ? this[x, y + 1] : null;
                cell.Left = x > 0 ? this[x - 1, y] : null;
            }
        }
    }
}