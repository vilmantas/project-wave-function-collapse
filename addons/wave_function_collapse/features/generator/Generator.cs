using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;

namespace WaveFunctionCollapse.Godot.Plugin;

[Tool]
public partial class Generator : Control
{
    [Export] public PackedScene Snapshotter;

    [Export] public PackedScene ui_GeneratorItemPrefab;

    [Export] public Control ctn_Imports;

    [Export] public LineEdit txt_ImportsLocation;

    [Export] public LineEdit txt_ExportDestination;

    [Export] public Button btn_LoadImports;

    private SnapshotsGenerator m_SnapshotsGenerator;

    public override void _Ready()
    {
        btn_LoadImports.Pressed += LoadImports;
    }

    private void LoadImports()
    {
        var children = ctn_Imports.GetChildren();

        foreach (var child in children)
        {
            child.QueueFree();
        }

        var files = DirAccess.GetFilesAt(txt_ImportsLocation.Text).Where(x => x.EndsWith(".glb")).ToList();

        var tiles = new List<PackedScene>();

        foreach (var fileName in files)
        {
            var fullPath = Path.Combine(txt_ImportsLocation.Text, fileName);

            tiles.Add(ResourceLoader.Load<PackedScene>(fullPath));

            var instance = ui_GeneratorItemPrefab.Instantiate<GeneratorItem>();

            var tileName = fileName.Split(".")[0];

            instance.Initialize(tileName, fullPath, this);

            ctn_Imports.AddChild(instance);
        }

        m_SnapshotsGenerator = Snapshotter.Instantiate<SnapshotsGenerator>();

        AddChild(m_SnapshotsGenerator);

        m_SnapshotsGenerator.Initialize(tiles.ToArray());

        m_SnapshotsGenerator.OnGeneratorFinished += HandleSnapshotsDone;

        GetTree().CreateTimer(0.25f).Timeout += m_SnapshotsGenerator.GenerateSnapshots;
    }

    private void HandleSnapshotsDone()
    {
        m_SnapshotsGenerator.OnGeneratorFinished -= HandleSnapshotsDone;

        m_SnapshotsGenerator.QueueFree();

        foreach (var node in ctn_Imports.GetChildren())
        {
            var x = node as GeneratorItem;

            x.LoadSnapshot();
        }
    }
}