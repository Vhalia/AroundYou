using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;

namespace AroundYou.Scripts.Components
{
    public partial class HealthComponent : Node2D
    {
        [Export]
        public StatsComponent StatsComponent;
        [Export]
        public PackedScene DeathEffectScene = null;
        [Export(PropertyHint.Range, "0.01,1,0.01")]
        public float ArmorRatio=1;

        [Node("HealthRegenerationTimer")]
        public Timer HealthRegenerationTimer;

        [Signal]
        public delegate void DiedEventHandler();
        [Signal]
        public delegate void HealthChangedEventHandler(float amount);
        [Signal]
        public delegate void DamagedEventHandler(float amount);

        private float _currentHealth;
        public float CurrentHealth { get => _currentHealth; set => SetCurrentHealth(value); }
        public bool IsDead => CurrentHealth <= 0;

        public override void _Ready()
        {
            this.WireNodes();
            CallDeferred(nameof(SetCurrentHealth), StatsComponent.MaxHealth);

            HealthRegenerationTimer.Timeout += HealthRegenerationTimer_Timeout;
        }


        public void LowerHealth(float amount)
        {
            if (HasEvaded())
            {
                GD.Print("Evaded");
                return;
            }
            float amountDamaged = ApplyArmor(amount);
            CurrentHealth -= amountDamaged;
            EmitSignal(SignalName.Damaged, amountDamaged);
            if (IsDead)
            {
                PlayDeathEffect();
                _ = EmitSignal(SignalName.Died);
            }
        }

        private bool HasEvaded()
        {
            return new RandomNumberGenerator().CanDoAction(StatsComponent.Evasion);
        }

        private float ApplyArmor(float amount)
        {
            return amount * ((float)StatsComponent.Armor/100);
        }

        private void PlayDeathEffect()
        {
            if (DeathEffectScene == null)
                return;

            Effect deathEffect = DeathEffectScene.Instantiate<Effect>();
            deathEffect.Init(Owner as Node2D, "deathEffectAnim");
        }

        public void SetCurrentHealth(float value)
        {
            _currentHealth = value;
            _ = EmitSignal(SignalName.HealthChanged, CurrentHealth);
        }

        #region Event handlers
        private void HealthRegenerationTimer_Timeout()
        {
            if (CurrentHealth + StatsComponent.HealthRegeneration > StatsComponent.MaxHealth)
                return;
            CurrentHealth += StatsComponent.HealthRegeneration;
        }

        #endregion
    }
}
