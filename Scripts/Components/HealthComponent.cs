using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int CurrentHealth { get { return _currentHealth; } set { SetCurrentHealth(value); } }
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
                    var deathEffect = DeathEffectScene.Instantiate<Effect>();
                    deathEffect.Init(Owner as Node2D, "deathEffectAnim");
                }
                EmitSignal(SignalName.Died);
            }
        }

        public void SetCurrentHealth(int value)
        {
            _currentHealth = value;                 
            EmitSignal(SignalName.HealthChanged, CurrentHealth);
        }
    }
}
