using Godot;
using System.Linq;

namespace AroundYou.Scripts.Components
{
    public partial class HurtboxComponent : Area2D
    {
        public void HandleCollision(Node other, int? damageAmount = null)
        {
            if (other is Character && damageAmount.HasValue)
                HandleCharacterCollision(damageAmount.Value);
        }

        private void HandleCharacterCollision(int damageAmount)
        {
            (Owner as Character).TakeDamage(damageAmount);
        }
    }
}
