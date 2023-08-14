using AroundYou.Scripts.Components;
using AroundYou.Utils.Attributes;
using Godot;

namespace AroundYou.Scripts
{
    public partial class ShooterEnemy : Enemy
    {
        [Node("Weapon")]
        public Weapon Weapon;

        public override void _Ready()
        {
            base._Ready();
        }

        public override void Chasing(Node2D other)
        {
            base.Chasing(other);
            Weapon.SetWeaponPositionAndRotation(Direction, GlobalPosition);
            Weapon.Shoot(Direction, GroupsToHit);
        }
    }
}
