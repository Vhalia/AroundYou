using AroundYou.Scripts;
using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;
using System;
using System.Collections.Generic;

public partial class SpawnerManager : Node2D
{
    [Export]
    public PackedScene ShooterEnemyScene;
    [Export]
    public PackedScene WalkerEnemyScene;
    [Export(PropertyHint.Range, "0,500")]
    public float SpawnDistanceFromPlayer;
    [Export]
    public bool ActivateSpawn = true;

    [Node("Timer")]
    public Timer Timer;

    public Player Player;
    public List<PackedScene> EnemiesScenes = new();

    public int NbEnemiesToSpawn = 3;

    public bool CanSpawnEnemies => Timer.IsStopped() && ActivateSpawn;

    public override void _Ready()
    {
        this.WireNodes();
        Player = GetTree().Root.GetNode("Main/Player") as Player;
        EnemiesScenes.Add(ShooterEnemyScene);
        EnemiesScenes.Add(WalkerEnemyScene);
    }

    public override void _Process(double delta)
    {
        if (CanSpawnEnemies)
        {
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        Random random = new();
        for (int i = 0; i < NbEnemiesToSpawn; i++)
        {
            Enemy shooterEnemy = SelectEnemyToSpawn(random);
            Vector2 x = GenerateRandomPositionAroundPlayer(random);
            shooterEnemy.Init(x);
            GetTree().CurrentScene.AddChild(shooterEnemy);
        }
        Timer.Start();
    }

    private Vector2 GenerateRandomPositionAroundPlayer(Random random)
    {
        float randomAngle = random.RandomBetween(0f, Mathf.Tau);
        return Player.GlobalPosition + (new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * SpawnDistanceFromPlayer);
    }

    private Enemy SelectEnemyToSpawn(Random random)
    {
        int randomNum = random.RandomBetween(0, 100);
        return randomNum <= 33 ? ShooterEnemyScene.Instantiate<Enemy>() : WalkerEnemyScene.Instantiate<Enemy>();
    }
}
