using AroundYou.Models;
using Godot;
using System;
using System.Collections.Generic;

public partial class DropComponent : Node2D
{
    private List<Drop> _possibleDrops;
    public override void _Ready()
    {
        base._Ready();
    }

    public List<Drop> Generate(int amount)
    {
        List<Drop> drop = new();
        for (int i = 0; i < amount; i++)
        {

        }
    }
}
