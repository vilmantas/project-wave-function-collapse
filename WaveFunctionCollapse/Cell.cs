using Godot;
using System;
using System.Collections.Generic;

namespace WaveFunctionCollapse
{
	public class Cell
	{
		public int X { get; init; }
		public int Y { get; init; }
		public Cell Up { get; set; }
		public Cell Right { get; set; }
		public Cell Down { get; set; }
		public Cell Left { get; set; }
		public List<Tile> Options { get; set; }

		public bool IsCollapsedOld { get; set; }
		public List<GodotTile> OptionsOld { get; set; }


		public int Entropy => Options.Count;
		public bool IsCollapsed => Options.Count == 1;

		public void Collapse()
		{
			if (IsCollapsed) return;

			var random = new Random();

			Options = new List<Tile> { Options[random.Next(Options.Count)] };
		}

		public void Collapse(int seed)
		{
			if (IsCollapsed) return;

			var random = new Random(seed);

			Options = new List<Tile> { Options[random.Next(Options.Count)] };
		}
	}
}