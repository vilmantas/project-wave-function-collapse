using Godot;
using System;

namespace WaveFunctionCollapse.Godot.Plugin.MapPreset;

[Tool]
public partial class MapGrid : Control
{
    [Export] public PackedScene CollumnScene;

    [Export] public Control ctn_Grid;

    public Action<MapTile> OnTilePressed;

    public Action<MapTile, int, int> OnTileInitialized;

    public void Initialize(int collumnCount, int rowCount)
    {
        for (int g = 0; g < ctn_Grid.GetChildCount(); g++)
        {
            ctn_Grid.GetChild(g).QueueFree();
        }

        for (int i = 0; i < rowCount; i++)
        {
            var instance = CollumnScene.Instantiate<MapGridRow>();

            ctn_Grid.AddChild(instance);

            instance.OnTilePressed += OnTilePressed;

            instance.OnTileInitialized += OnTileInitialized;

            instance.Initialize(i, collumnCount);
        }
    }

    public string PrintGrid()
    {
        string grid = string.Empty;

        for (int i = 0; i < ctn_Grid.GetChildCount(); i++)
        {
            var column = ctn_Grid.GetChild(i) as MapGridRow;

            for (int j = 0; j < column!.GetChildCount(); j++)
            {
                var tile = column.GetChild(j) as MapTile;

                grid += $"{tile!.Id} ";
            }

            grid += "\n";
        }

        GD.Print(grid);

        return grid;
    }
}
