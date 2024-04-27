using Godot;
using System;

[Tool]
public partial class TileSnapshotter : Node
{
    [Export] public SubViewport TextureViewport;

    private bool Initialized = false;

    public string SavePath;

    private float delay = 0f;

    public Action OnSaved;

    public override void _Process(double delta)
    {
        delay += (float)delta;

        if (delay < 1f) return;

        if (Initialized) return;

        Initialized = true;

        TextureViewport.GetViewport().GetTexture().GetImage().SavePng(SavePath);

        OnSaved?.Invoke();
    }

    public void Initialize(string snapshotSavePath)
    {
        SavePath = snapshotSavePath;
    }
}
