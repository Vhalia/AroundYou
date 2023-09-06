using AroundYou.Scripts;
using AroundYou.Scripts.Components;
using AroundYou.Scripts.States;
using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;

public partial class Enemy : Character
{
    [Node("HurtboxComponent")]
    public HurtboxComponent HurtboxComponent;
    [Node("FloatingTextComponent")]
    public FloatingTextComponent FloatingTextComponent;

    public Player Player;

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        Player = GetTree().Root.GetNode("Main/Player") as Player;

        HurtboxComponent.BodyShapeEntered += HurtboxComponent_BodyShapeEntered;
        HealthComponent.Died += HealthComponent_Died;
        HealthComponent.Damaged += HealthComponent_Damaged;

        StateMachine.ChangeState(GetNodeOrNull<State>("StateMachine/IDLE"));
    }


    public override void _Process(double delta)
    {
        base._Process(delta);
        Chasing(Player);
    }

    public void Init(Vector2 spawnPosition)
    {
        GlobalPosition = spawnPosition;
    }

    public virtual void Chasing(Node2D other)
    {
        Direction = (other.GlobalPosition - GlobalPosition).Normalized();
    }

    private void HealthComponent_Died()
    {
        QueueFree();
    }

    private void HurtboxComponent_BodyShapeEntered(Rid bodyRid, Node2D other, long bodyShapeIndex, long localShapeIndex)
    {
        HurtboxComponent.HandleCollision(other, ContactHitDamage);
    }
    private void HealthComponent_Damaged(float amount)
    {
        FloatingTextComponent.DisplayDamageNumber(amount);
    }
}
