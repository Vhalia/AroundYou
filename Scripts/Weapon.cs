using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;
using System;
using System.Collections.Generic;

public partial class Weapon : Node2D
{
    [Node("FiringRateTimer")]
    public Timer FiringRateTimer;
    [Node("ReloadTimer")]
    public Timer ReloadTimer;
    [Node("BulletSpawnMarker")]
    public Marker2D BulletSpawnMarker;
    [Node("AnimatedSprite2D")]
    public AnimatedSprite2D AnimatedSprite;
    [Node("StatsComponent")]
    public StatsComponent StatsComponent;

    [Export]
    public PackedScene BulletScene;

    public Stack<Bullet> Bullets;

    public int _munitionsInMagazine;
    public int MunitionsInMagazine { get => _munitionsInMagazine; set => SetMunitionsInMagazine(value); }
    public bool CanShoot => MunitionsInMagazine > 0 && FiringRateTimer.IsStopped() && ReloadTimer.IsStopped();

    [Signal]
    public delegate void BulletsInMagazineChangedEventHandler(int bulletsCount);
    [Signal]
    public delegate void ReloadingEventHandler(int reloadTime);

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        Bullets = new Stack<Bullet>();
        FillBullets();

        FiringRateTimer.WaitTime = DetermineFireRateTime();
        ReloadTimer.WaitTime = StatsComponent.ReloadTime;

        ReloadTimer.Timeout += OnReloadTimerTimeout;

        CallDeferred(nameof(SetMunitionsInMagazine), StatsComponent.MagazineCapacity);
    }

    public void Shoot(Vector2 Direction)
    {
        if (CanShoot)
        {
            AnimatedSprite?.Play("shoot");
            for (int i = 0; i < StatsComponent.BulletsPerShot; i++)
            {
                Bullet bullet = Bullets.Pop();
                bullet.Init(StatsComponent.Damage, StatsComponent.BulletSpeed, Direction, BulletSpawnMarker.GlobalPosition);
                GetTree().CurrentScene.AddChild(bullet);
            }
            MunitionsInMagazine--;

            FiringRateTimer.Start();
        }
        else if (MunitionsInMagazine == 0 && ReloadTimer.IsStopped())
        {
            Reload();
        }

    }

    public void Reload()
    {
        _ = EmitSignal(SignalName.Reloading, StatsComponent.ReloadTime);
        ReloadTimer.Start();
    }

    public void SetWeaponPositionAndRotation(Vector2 direction, Vector2 origin)
    {
        float directionAngle = direction.Angle();
        GlobalPosition = origin + (new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle)) * 10f);
        //GlobalPosition = origin + direction * 10f; //same as

        FlipWeapon(directionAngle);

        Rotation = directionAngle;
    }

    private void FlipWeapon(float directionAngle)
    {
        if (Mathf.Abs(directionAngle) > MathF.PI / 2)
        {
            if (Mathf.Sign(Scale.Y) == 1)
            {
                Scale = new Vector2(Scale.X, -Scale.Y);
            }
        }
        else
        {
            Scale = new Vector2(Scale.X, Mathf.Abs(Scale.Y));
        }
    }

    private void FillBullets()
    {
        MunitionsInMagazine = StatsComponent.MagazineCapacity;
        for (int i = 0; i < StatsComponent.MagazineCapacity; i++)
        {
            Bullet bullet = BulletScene.Instantiate<Bullet>();
            Bullets.Push(bullet);
        }
    }

    private void SetMunitionsInMagazine(int value)
    {
        _munitionsInMagazine = value;
        _ = EmitSignal(SignalName.BulletsInMagazineChanged, MunitionsInMagazine);
    }

    private void OnReloadTimerTimeout()
    {
        FillBullets();
    }

    //
    // Summary:
    //     Allow to get number of seconds between each shot
    //     ShotSpeed is the amount of bullets shot per seconds
    private double DetermineFireRateTime()
    {
        return (1000 / (double)StatsComponent.ShotSpeed)/1000;
    }
}
