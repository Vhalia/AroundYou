using AroundYou.Scripts;
using AroundYou.Utils.Extensions;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Model = AroundYou.Models;

public partial class DropComponent : Node2D
{
    [Export]
    private string _basePath;
    [Export]
    private int distanceSpawn;

    private List<Model.Drop> _possibleDrops;

    public override void _Ready()
    {
        base._Ready();

        _possibleDrops = new()
        {
            new Model.Drop()
            {
                ChanceToDrop = 0.5f,
                Rarity = Model.Enums.ERarity.COMMON,
                PackedScene = GD.Load<PackedScene>(_basePath + "blueGem.tscn")
            }
        };
    }

    public List<Model.Drop> Generate(int amount, Vector2? position = null, bool aroundPosition = false)
    {
        List<Model.Drop> drops = new();
        foreach (Model.Drop possibleDrop in _possibleDrops)
        {
            for(int i = 0; i < amount; i++)
            {
                if (new RandomNumberGenerator().CanDoAction(possibleDrop.ChanceToDrop))
                {
                    drops.Add(possibleDrop);

                    var scene = possibleDrop.PackedScene.Instantiate<Drop>();

                    scene.Init(
                        aroundPosition?
                        GenerateRandomPositionAround(position.Value, distanceSpawn)
                        : position.Value);

                    GetTree().CurrentScene.CallDeferred("add_child", scene);
                }
            }
        }
        return drops;
    }

    private Vector2 GenerateRandomPositionAround(Vector2 position, int distance = 1)
    {
        float randomAngle = new RandomNumberGenerator().RandfRange(0f, Mathf.Tau);
        return position + (new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * distance);
    }
}
