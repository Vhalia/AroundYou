using AroundYou.Scripts.Components;
using AroundYou.Scripts.Singleton;
using AroundYou.Scripts.States;
using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;
using System;
using System.Drawing;
using System.Reflection.Metadata;

namespace AroundYou.Scripts;

public partial class Player : Character
{
    [Node("HurtboxComponent")]
    public HurtboxComponent HurtboxComponent;
    [Node("Weapon")]
    public Weapon Weapon;
    [Node("AnimationPlayer")]
    public AnimationPlayer AnimationPlayer;
    [Node("UI/ReloadBar")]
    public ReloadBar ReloadBar;
    [Node("Sprite2D")]
    public Sprite2D Sprite;

    [Export(hint: PropertyHint.ArrayType)]
    public string[] GroupsToHit = new[] { "" };


    public Vector2 AimingDirection;

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        StateMachine.ChangeState(GetNodeOrNull<State>("StateMachine/IDLE"));
        HurtboxComponent.BodyShapeEntered += HurtboxComponent_BodyShapeEntered;
        Weapon.Reloading += Weapon_Reloading;
        Weapon.BulletsInMagazineChanged += Weapon_BulletsInMagazineChanged;
        HealthComponent.HealthChanged += HealthComponent_HealthChanged;

        //Force call because weapon's ready is call before player's ready
        Weapon_BulletsInMagazineChanged(Weapon.MunitionsInMagazine);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Direction = new Vector2(Input.GetAxis("ui_left", "ui_right"), Input.GetAxis("ui_up", "ui_down")).Normalized();
        AimingDirection = (GetGlobalMousePosition() - this.GlobalPosition).Normalized();
        Weapon.SetWeaponPositionAndRotation(AimingDirection, GlobalPosition);
        if (Input.IsActionJustPressed("shoot"))
        {
            Shoot(AimingDirection);
        }
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        AnimationPlayer.Play("hurt");
    }

    public void Shoot(Vector2 direction)
    {
        Weapon.Shoot(direction, GroupsToHit);
    }

    public override void SetFacingDirection()
    {
        if (AimingDirection.X > 0)
        {
            Sprite.Scale = new Vector2(1, 1);
            return;
        }
        if (AimingDirection.X < 0)
        {
            Sprite.Scale = new Vector2(-1, 1);
            return;
        }

        base.SetFacingDirection();
    }

    #region EventHandlers
    private void HurtboxComponent_BodyShapeEntered(Rid bodyRid, Node2D other, long bodyShapeIndex, long localShapeIndex)
    {
        HurtboxComponent.HandleEntityCollision(other, 1);
    }
    private void Weapon_BulletsInMagazineChanged(int bulletsCount)
    {
        this.GetAutoLoad<EventsBus>().EmitSignal(EventsBus.SignalName.BulletsInMagazineChanged, bulletsCount, Weapon.MagazineCapacity);
    }

    private void Weapon_Reloading(int reloadTime)
    {
        ReloadBar.Play(reloadTime);
    }

    private void HealthComponent_HealthChanged(int amount)
    {
        this.GetAutoLoad<EventsBus>().EmitSignal(EventsBus.SignalName.PlayerHealthChanged, amount);

    }

    #endregion
}
