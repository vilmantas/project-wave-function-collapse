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

		public int Entropy => Options.Count;
		public bool IsCollapsed => Options.Count == 1;

		public void Collapse()
		{
			if (IsCollapsed) return;

			var tickets = Options.Sum(x => x.Weight);

			var roll = Random.Next(1, tickets);

			var sum = 0;

			foreach (var option in Options)
			{
				sum += option.Weight;

				if (roll <= sum)
				{
					Options = new List<Tile> { option };
					break;
				}
			}

			Options = new List<Tile> { Options[Random.Next(Options.Count)] };
		}
	}
}