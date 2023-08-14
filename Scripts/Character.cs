using AroundYou.Scripts.Components;
using AroundYou.Scripts.States;
using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;

namespace AroundYou.Scripts
{
    public partial class Character : CharacterBody2D
    {
        [Node("StateMachine")]
        public StateMachine StateMachine;
        [Node("HealthComponent")]
        public HealthComponent HealthComponent;
        [Node("Sprite2D")]
        public Sprite2D Sprite;


        public Vector2 Direction = Vector2.Zero;

        public override void _Ready()
        {
            this.WireNodes();
        }

        public virtual void TakeDamage(int amount)
        {
            HealthComponent.LowerHealth(amount);
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            SetFacingDirection();
        }

        public virtual void SetFacingDirection()
        {
            if (Direction.X > 0)
            {
                Sprite.Scale = new Vector2(1, 1);
                return;
            }
            if (Direction.X < 0)
            {
                Sprite.Scale = new Vector2(-1, 1);
                return;
            }
        }
    }
}
