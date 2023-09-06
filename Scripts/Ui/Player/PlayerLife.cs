using AroundYou.Scripts.Singleton;
using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;

public partial class PlayerLife : Control
{
    [Node("HBoxContainer/CurrentHealth")]
    private Label _currentHealth;
    [Node("HBoxContainer/MaxHealth")]
    private Label _maxHealth;

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        this.GetAutoLoad<EventsBus>().PlayerHealthChanged += OnPlayerHealthChanged;

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
