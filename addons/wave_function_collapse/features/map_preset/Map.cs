using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WaveFunctionCollapse.Godot.Plugin.MapPreset;

[Tool]
public partial class Map : VBoxContainer
{
    [Export] public FileDialog fd_LoadPreset;

    [Export] LineEdit txt_ImportLocation;

    [Export] Button btn_Import;

    [Export] Control ctn_AvailableTiles;

    public List<GodotTile> LoadedTiles = new ();

    public List<TilePreview> LoadedPreviews = new ();

    [Export] PackedScene TileScene;

    [Export] public MapGrid ctn_grid;

    [Export] public LineEdit txt_Rows;

    [Export] public LineEdit txt_Columns;

    [Export] public Button btn_GenerateGrid;

    [Export] public Button btn_PrintGrid;

    [Export] public Button btn_LoadPreset;

    public TilePreview SelectedTile;

    public override void _Ready()
    {
        btn_Import.Pressed += LoadTiles;

        btn_GenerateGrid.Pressed += () => GenerateGrid(int.Parse(txt_Columns.Text), int.Parse(txt_Rows.Text), null);

        btn_PrintGrid.Pressed += () =>
        {
            var result = ctn_grid.PrintGrid();

            var preset = new GodotMapPreset
            {
                Preset = result
            };

            var tiles = new GodotTiles()
            {
                Tiles = LoadedTiles.ToArray()
            };

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            var mapPreset = "map-preset-" + timestamp + ".tres";

            var tilesFileName = "tiles-" + timestamp + ".tres";

            SaveResource(mapPreset, preset);

            SaveResource(tilesFileName, tiles);
        };

        btn_LoadPreset.Pressed += () =>
        {
            fd_LoadPreset.CurrentDir = txt_ImportLocation.Text;
            fd_LoadPreset.FileMode = FileDialog.FileModeEnum.OpenFile;
            fd_LoadPreset.Visible = true;
        };

        fd_LoadPreset.FileSelected += path =>
        {
            var preset = ResourceLoader.Load<GodotMapPreset>(path);

            LoadPreset(preset.Preset);
        };
    }

    private void LoadPreset(string preset)
    {
        var grid = preset.Split('\n')
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(line => line.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray()).ToArray();

        foreach (var row in grid)
        {
            foreach (var tile in row)
            {
                if (tile == -1) continue;

                if (LoadedTiles.All(x => x.Id != tile))
                {
                    GD.Print(
                        $"Tile {tile} in preset not found in loaded tiles. Aborting preset load.");

                    return;
                }
            }
        }

        void OnInitializedCallback(MapTile tile, int x, int y)
        {
            var presetId = grid[y][x];

            if (presetId == -1) return;

            var t = LoadedPreviews.First(preview => int.Parse(preview.lbl_Id.Text) == presetId);

            tile.SetTile(t.img_Preview.Texture, (int) t.img_Preview.RotationDegrees, presetId);
        }

        GenerateGrid(grid[0].Length, grid.Length, OnInitializedCallback);
    }

    private static void SaveResource(string fileName, Resource preset)
    {
        var presetFilePath = "res://" + fileName;

        ResourceSaver.Save(preset, presetFilePath);
    }

    private void GenerateGrid(int cols, int rows, Action<MapTile, int, int> onInitializedCallback)
    {
        ctn_grid.OnTileInitialized = null;
        ctn_grid.OnTilePressed = null;

        ctn_grid.OnTilePressed += AssignTile;

        if (onInitializedCallback != null)
        {
            ctn_grid.OnTileInitialized += onInitializedCallback;
        }

        ctn_grid.Initialize(cols, rows);
    }

    public void AssignTile(MapTile tile)
    {
        if (SelectedTile == null) return;

        tile.SetTile(SelectedTile.img_Preview.Texture, (int)SelectedTile.img_Preview.RotationDegrees, int.Parse(SelectedTile.lbl_Id.Text));
    }

    private void LoadTiles()
    {
        var tilePath = txt_ImportLocation.Text;

        var tiles = DirAccess.GetFilesAt(tilePath);

        LoadedTiles.Clear();

        LoadedPreviews.Clear();

        foreach (var tileFile in tiles)
        {
            var tileResource = ResourceLoader.Load<GodotTile>(Path.Combine(tilePath, tileFile));

            var tileName = tileFile.Split('.')[0].Split('-')[0];

            var instance = TileScene.Instantiate<TilePreview>();

            var snapshotPath = Path.Combine(Main.SNAPSHOTS_PATH, tileName + "-snapshot.png");

            instance.OnTilePressed += (tile) =>
            {
                SelectedTile = tile;
            };

            instance.Initialize(tileName, tileResource.Id.ToString(), snapshotPath,
                tileResource.RotationY);

            ctn_AvailableTiles.AddChild(instance);

            LoadedTiles.Add(tileResource);

            LoadedPreviews.Add(instance);
        }
    }
}
