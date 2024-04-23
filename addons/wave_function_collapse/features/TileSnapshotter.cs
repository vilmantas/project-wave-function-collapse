using Godot;
using System;

[Tool]
public partial class TileSnapshotter : Node
{
    [Export] public SubViewport TextureViewport;

    private bool Initialized = false;

    private string savepath;

    private float delay = 0f;

    public override void _Process(double delta)
    {
        delay += (float)delta;

        if (delay < 0.5f) return;

        if (Initialized) return;

        Initialized = true;

        TextureViewport.GetViewport().GetTexture().GetImage().SavePng(savepath);

        GetParent().QueueFree();

        QueueFree();
    }

    public void Initialize(string snapshotSavePath)
    {
        savepath = snapshotSavePath;
    }
}
