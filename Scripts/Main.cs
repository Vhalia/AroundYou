using AroundYou.Utils.Extensions;
using Godot;
using System;

public partial class Main : Node2D
{
    public ulong? seed;
    public override void _Ready()
    {
        base._Ready();
        if (seed != null)
        {
            GD.Seed(seed.Value);
        }
        GD.Randomize();
    }
}
