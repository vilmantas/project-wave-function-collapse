using System.IO;
using System.Linq;
using Godot;

namespace WaveFunctionCollapse.Godot.Plugin;

[Tool]
public partial class Generator : Control
{
    [Export] public PackedScene ui_GeneratorItemPrefab;

    [Export] public Control ctn_Imports;

    [Export] public LineEdit txt_ImportsLocation;

    [Export] public LineEdit txt_ExportDestination;

    [Export] public Button btn_LoadImports;

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

        foreach (var fileName in files)
        {
            var fullPath = Path.Combine(txt_ImportsLocation.Text, fileName);

            var instance = ui_GeneratorItemPrefab.Instantiate<GeneratorItem>();

            GetTree().CreateTimer(1f * files.IndexOf(fileName)).Timeout += () =>
            {
                instance.Initialize(fileName, fullPath, this, true);
            };

            ctn_Imports.AddChild(instance);
        }
    }
}