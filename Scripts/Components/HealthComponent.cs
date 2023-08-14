using AroundYou.Utils.Extensions;
using Godot;

namespace AroundYou.Scripts.Components
{
    public partial class HealthComponent : Node2D
    {
        [Export]
        public int MaxHealth = 20;
        [Export]
        public PackedScene DeathEffectScene = null;

        [Signal]
        public delegate void DiedEventHandler();
        [Signal]
        public delegate void HealthChangedEventHandler(int amount);

        private int _currentHealth;
        public int CurrentHealth { get => _currentHealth; set => SetCurrentHealth(value); }
        public bool IsDead => CurrentHealth <= 0;

        public override void _Ready()
        {
            this.WireNodes();
            CurrentHealth = MaxHealth;
        }


        public void LowerHealth(int amount)
        {
            CurrentHealth -= amount;
            if (IsDead)
            {
                if (DeathEffectScene != null)
                {
                    Effect deathEffect = DeathEffectScene.Instantiate<Effect>();
                    deathEffect.Init(Owner as Node2D, "deathEffectAnim");
                }
                _ = EmitSignal(SignalName.Died);
            }
        }

        public void SetCurrentHealth(int value)
        {
            _currentHealth = value;
            _ = EmitSignal(SignalName.HealthChanged, CurrentHealth);
        }
    }
}
