using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WaveFunctionCollapse
{
	public class Cell
	{
		public Random Random { get; set; } = new Random();
		public int X { get; init; }
		public int Y { get; init; }
		public Cell Up { get; set; }
		public Cell Right { get; set; }
		public Cell Down { get; set; }
		public Cell Left { get; set; }
		public List<Tile> Options { get; set; }

		public bool IsCollapsed = false;
		public int Entropy => Options.Count;
		public bool IsBroken => Options.Count == 0;
		public Cell[] Neighbours => new Cell[] { Up, Right, Down, Left };
	}
}