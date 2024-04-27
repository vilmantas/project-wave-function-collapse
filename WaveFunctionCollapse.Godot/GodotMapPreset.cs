using Godot;

namespace WaveFunctionCollapse.Godot;

[GlobalClass, Tool]
public partial class GodotMapPreset : Resource
{
    [Export] public string Preset;
}