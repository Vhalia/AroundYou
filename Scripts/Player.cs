using AroundYou.Scripts.Components;
using AroundYou.Scripts.Singleton;
using AroundYou.Scripts.States;
using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;
using System.Runtime.CompilerServices;

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
    public new Sprite2D Sprite;
    [Node("PickupArea")]
    public Area2D PickupArea;

    public Vector2 AimingDirection;

    [Signal]
    public delegate void PickedupEventHandler();

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        StateMachine.ChangeState(GetNodeOrNull<State>("StateMachine/IDLE"));
        HurtboxComponent.BodyShapeEntered += HurtboxComponent_BodyShapeEntered;
        Weapon.Reloading += Weapon_Reloading;
        Weapon.BulletsInMagazineChanged += Weapon_BulletsInMagazineChanged;
        HealthComponent.HealthChanged += HealthComponent_HealthChanged;
        HealthComponent.Damaged += HealthComponent_Damaged;
        PickupArea.BodyShapeEntered += PickupArea_BodyShapeEntered;
        PickupArea.AreaShapeEntered += PickupArea_AreaShapeEntered;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Direction = new Vector2(Input.GetAxis("ui_left", "ui_right"), Input.GetAxis("ui_up", "ui_down")).Normalized();
        AimingDirection = (GetGlobalMousePosition() - GlobalPosition).Normalized();
        Weapon.SetWeaponPositionAndRotation(AimingDirection, GlobalPosition);
        if (Input.IsActionJustPressed("shoot"))
        {
            Shoot(AimingDirection);
        }
    }

    public void Shoot(Vector2 direction)
    {
        Weapon.Shoot(direction);
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

    public void LevelUp(int level)
    {

    }

    private void Pickup(Drop drop)
    {
        EmitSignal(SignalName.Pickedup);
        drop.QueueFree();
    }

    #region TweenAnimations

    private void HurtAnimation()
    {
        var tween = CreateTween();
        tween.SetLoops(3);

        tween.TweenProperty(Sprite.Material, "shader_parameter/state", 1, 0.15);
        tween.TweenProperty(Sprite.Material, "shader_parameter/state", 0, 0.15);
    }

    #endregion

    #region EventHandlers
    private void HurtboxComponent_BodyShapeEntered(Rid bodyRid, Node2D other, long bodyShapeIndex, long localShapeIndex)
    {
        HurtboxComponent.HandleCollision(other, ContactHitDamage);
    }
    private void Weapon_BulletsInMagazineChanged(int bulletsCount)
    {
        _ = this.GetAutoLoad<EventsBus>().EmitSignal(EventsBus.SignalName.BulletsInMagazineChanged, bulletsCount, Weapon.StatsComponent.MagazineCapacity);
    }

    private void Weapon_Reloading(int reloadTime)
    {
        ReloadBar.Play(reloadTime);
    }

    private void HealthComponent_HealthChanged(float amount)
    {
        _ = this.GetAutoLoad<EventsBus>().EmitSignal(EventsBus.SignalName.PlayerHealthChanged, amount);
    }

    private void HealthComponent_Damaged(float amount)
    {
        HurtAnimation();
    }

    private void PickupArea_BodyShapeEntered(Rid bodyRid, Node2D body, long bodyShapeIndex, long localShapeIndex)
    {
        if (body is Drop)
            Pickup(body as Drop);
    }

    private void PickupArea_AreaShapeEntered(Rid areaRid, Area2D area, long areaShapeIndex, long localShapeIndex)
    {
        var parent = area.Owner;
        if (parent is Drop)
            Pickup(parent as Drop);
    }

    #endregion
}
