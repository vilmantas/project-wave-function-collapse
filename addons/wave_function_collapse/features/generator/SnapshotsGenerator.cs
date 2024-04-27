using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WaveFunctionCollapse.Godot.Plugin;

[Tool]
public partial class SnapshotsGenerator : Node3D
{
    [Export] public PackedScene[] Tiles;

    [Export] public Camera3D Camera;

    [Export] public SubViewport TextureViewport;

    public List<(Vector3 position, string file)> Positions = new();

    public int CurrentPositionIndex = 0;

    public Action OnGeneratorFinished;

    public void Initialize(PackedScene[] tiles)
    {
        Tiles = tiles;

        foreach (var tile in Tiles)
        {
            var instance = tile.Instantiate<Node3D>();

            AddChild(instance);

            var position = new Vector3(0, 0, Tiles.ToList().IndexOf(tile) * -2);

            Positions.Add((position, tile.ResourcePath));

            instance.Position = position;
        }
    }

    public void GenerateSnapshots()
    {
        for (int i = 0; i < Positions.Count; i++)
        {
            var index = i;

            GetTree().CreateTimer(i * 0.1f).Timeout += () =>
            {
                var path = Path.Combine(Main.SNAPSHOTS_PATH, Positions[index].file.Split("/").Last().Split(".").First());

                TextureViewport.GetViewport().GetTexture().GetImage()
                    .SavePng($"{path}-snapshot.png");

                var nextPositionIndex = (index + 1) % Positions.Count;

                if (nextPositionIndex == 0) OnGeneratorFinished?.Invoke();

                Camera.GlobalPosition = new Vector3(Positions[nextPositionIndex].position.X,
                    Camera.GlobalPosition.Y, Positions[nextPositionIndex].position.Z);
            };
        }
    }
}
