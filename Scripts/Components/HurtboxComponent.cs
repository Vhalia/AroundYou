using Godot;

namespace AroundYou.Scripts.Components
{
    public partial class HurtboxComponent : Area2D
    {
        [Export]
        public HealthComponent HealthComponent;

        public void HandleEntityCollision(Node other, int damageAmount)
        {
            if (other == GetParent())
            {
                return;
            }

            HealthComponent.LowerHealth(damageAmount);
        }
    }
}
