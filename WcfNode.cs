using Godot;
using System;
using System.Collections.Generic;

public partial class WcfNode : Node
{
	public bool IsCollapsed { get; set; }
	public List<WcfTile> Options { get; set; }
}
