using AroundYou.Scripts;
using AroundYou.Scripts.Singleton;
using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;

public partial class PlayerLife : Control
{
    [Export]
    private Player _player;

    [Node("HBoxContainer/CurrentHealth")]
    private Label _currentHealth;
    [Node("HBoxContainer/MaxHealth")]
    private Label _maxHealth;

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        this.GetAutoLoad<EventsBus>().PlayerHealthChanged += OnPlayerHealthChanged;
        _player.StatsComponent.MaxHealthChanged += StatsComponent_MaxHealthChanged;
    }

    private void StatsComponent_MaxHealthChanged(int maxHealth)
    {
        _maxHealth.Text = maxHealth.ToString();
    }

    private void OnPlayerHealthChanged(float amount)
    {
        if (string.IsNullOrEmpty(_maxHealth.Text))
        {
            _maxHealth.Text = amount.ToString();
        }

        _currentHealth.Text = amount.ToString();
    }
}
