using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using WaveFunctionCollapse.Godot;

namespace WaveFunctionCollapse.Godot.Plugin;

[Tool]
public partial class Main : Control
{
    public static string ARTIFACTS_PATH = "res://addons/wave_function_collapse/artifacts";

    public static string SNAPSHOTS_PATH = Path.Combine(ARTIFACTS_PATH, "snapshots");
}
