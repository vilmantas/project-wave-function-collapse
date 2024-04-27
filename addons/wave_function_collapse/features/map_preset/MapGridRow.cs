using Godot;
using System;

namespace WaveFunctionCollapse.Godot.Plugin.MapPreset;

[Tool]
public partial class MapGridRow : Control
{
    [Export] public PackedScene TileScene;

    public Action<MapTile> OnTilePressed;

    public Action<MapTile, int, int> OnTileInitialized;

    public void Initialize(int rowIndex, int tileCount)
    {
        for (int i = 0; i < tileCount; i++)
        {
            var instance = TileScene.Instantiate<MapTile>();

            AddChild(instance);

            instance.OnTilePressed += OnTilePressed;

            instance.Initialize($"{i},{rowIndex}");

            OnTileInitialized?.Invoke(instance, i, rowIndex);
        }
    }
}
