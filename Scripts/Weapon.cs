using AroundYou.Scripts;
using AroundYou.Scripts.Singleton;
using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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

    [Export]
    public PackedScene BulletScene;
    [Export]
    public float FiringRate = 1f;
    [Export]
    public int NumberOfBulletsPerShot = 1;
    [Export]
    public int MagazineCapacity = 30;
    [Export]
    public float ReloadTime = 1f;
    [Export]
    public int Damage = 1;
    [Export]
    public float BulletSpeed = 3000f;


    public Stack<Bullet> Bullets;

    public int _munitionsInMagazine;
    public int MunitionsInMagazine { get { return _munitionsInMagazine; } set { SetMunitionsInMagazine(value); } }
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

        FiringRateTimer.WaitTime = FiringRate;
        ReloadTimer.WaitTime = ReloadTime;

        ReloadTimer.Timeout += OnReloadTimerTimeout;
    }

    public void Shoot(Vector2 Direction, params string[] groupsToHit)
    {
        if (CanShoot)
        {
            AnimatedSprite?.Play("shoot");
            for (var i = 0; i < NumberOfBulletsPerShot; i++)
            {
                Bullet bullet = Bullets.Pop();
                bullet.Init(Damage, BulletSpeed, Direction, BulletSpawnMarker.GlobalPosition, groupsToHit);
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
        EmitSignal(SignalName.Reloading, ReloadTime);
        ReloadTimer.Start();
    }

    public void SetWeaponPositionAndRotation(Vector2 direction, Vector2 origin)
    {
        var directionAngle = direction.Angle();
        GlobalPosition = origin + new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle)) * 10f;
        //GlobalPosition = origin + direction * 10f; //same as
        FlipWeapon(directionAngle);

        Rotation = directionAngle;
    }

    private void FlipWeapon(float directionAngle)
    {
        if (Mathf.Abs(directionAngle) > MathF.PI / 2)
        {
            if (Mathf.Sign(Scale.Y) == 1)
                Scale = new Vector2(Scale.X, -Scale.Y);
        }
        else
            Scale = new Vector2(Scale.X, Mathf.Abs(Scale.Y));
    }

    private void FillBullets()
    {
        MunitionsInMagazine = MagazineCapacity;
        for (int i = 0; i < MagazineCapacity; i++)
        {
            Bullet bullet = BulletScene.Instantiate<Bullet>();
            Bullets.Push(bullet);
        }
    }

    private void SetMunitionsInMagazine(int value)
    {
        _munitionsInMagazine = value;
        EmitSignal(SignalName.BulletsInMagazineChanged, MunitionsInMagazine);
    }

    private void OnReloadTimerTimeout()
    {
        FillBullets();
    }
}
