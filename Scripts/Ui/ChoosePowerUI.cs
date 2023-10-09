using AroundYou.Models;
using AroundYou.Models.Modifiers;
using AroundYou.Scripts;
using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ChoosePowerUI : Control
{
    [Export]
    private Player _player;

    [Node("MarginContainer/HBoxContainer/Power1")]
    private Control _powerControl1;
    [Node("MarginContainer/HBoxContainer/Power2")]
    private Control _powerControl2;
    [Node("MarginContainer/HBoxContainer/Power3")]
    private Control _powerControl3;

    private List<Power> _power;
    private Power[] _powersShown;

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        _power = new List<Power>()
        {
            Power.Init("Yummy !").AddModifier(new StatModifier("Increase max life by #%", 5,
                (value) => _player.StatsComponent.IncreasePercent(nameof(StatsComponent.MaxHealth), value))),

            Power.Init("Rock").AddModifier(new StatModifier("Add +#% armor", 5,
                (value) => _player.StatsComponent.Add(nameof(StatsComponent.Armor), value))),

            Power.Init("Ninja").AddModifier(new StatModifier("Add +#% evasion", 5,
                (value) => _player.StatsComponent.Add(nameof(StatsComponent.Evasion), value))),

            Power.Init("Speedy").AddModifier(new StatModifier("Increase movement speed by #%", 2,
                (value) => _player.StatsComponent.IncreasePercent(nameof(StatsComponent.Evasion), value)))
        };

        _powersShown = new Power[3];

        _powerControl1.GuiInput += (@event) => PowerControlGuiInput(@event, 0);
        _powerControl2.GuiInput += (@event) => PowerControlGuiInput(@event, 1);
        _powerControl3.GuiInput += (@event) => PowerControlGuiInput(@event, 2);
    }

    public void ShowModifiers()
    {
        List<Power> powers = SelectRandomPower();
        _powersShown = powers.ToArray();

        InitModifierControl(_powerControl1, powers[0]);
        InitModifierControl(_powerControl2, powers[1]);
        InitModifierControl(_powerControl3, powers[2]);

        Show();
    }

    private void InitModifierControl(Control controlPower, Power power)
    {
        var powerName = controlPower.GetNode("VBoxContainer/Name") as Label;
        powerName.Text = power.Name;

        var powerDescription = controlPower.GetNode("VBoxContainer/Description") as Label;
        powerDescription.Text = power.GenerateDescription();
    }

    private List<Power> SelectRandomPower()
    {
        List<Power> powersChosen = new();
        List<int> indexes = new();

        for (int i = 0; i < 3; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = new Random().RandomBetween(0, _power.Count - 1);
            } while (indexes.Contains(randomIndex));

            indexes.Add(randomIndex);
            powersChosen.Add(_power[randomIndex]);
        }

        return powersChosen;
    }

    private void ChoosePower(int powerControlIndex)
    {
        var power = _powersShown[powerControlIndex];

        power.ApplyModifiers();

        Hide();
    }

    private void PowerControlGuiInput(InputEvent @event, int powerControlIndex)
    {
        if (@event is InputEventMouseButton mouseInput)
        {
            if (mouseInput.ButtonIndex == MouseButton.Left && mouseInput.Pressed)
            {
                ChoosePower(powerControlIndex);
            }
        }
    }
}
