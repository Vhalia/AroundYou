using AroundYou.Models;
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

    [Node("MarginController/HBoxContainer/Power1")]
    private Control _power1;
    [Node("MarginController/HBoxContainer/Power2")]
    private Control _power2;
    [Node("MarginController/HBoxContainer/Power3")]
    private Control _power3;

    private List<Power> _powers;

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        _powers = new()
        {
            new()
            {
                Name = "Yummy !",
                DescriptionTemplate = "Increase maximum life by #%",
                Values = new() { 5 }
            }
        };

        _power1.GuiInput += (@event) => PowerControlGuiInput(@event, _power1);
        _power2.GuiInput += (@event) => PowerControlGuiInput(@event, _power2);
        _power3.GuiInput += (@event) => PowerControlGuiInput(@event, _power3);
    }

    public void ShowPowers()
    {
        List<Power> powers = SelectRandomPower();

        InitPowerControl(_power1, powers[0]);
        InitPowerControl(_power2, powers[1]);
        InitPowerControl(_power3, powers[2]);

        Show();
    }

    private void InitPowerControl(Control controlPower, Power power)
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
                randomIndex = new Random().RandomBetween(0, _powers.Count - 1);
            } while (indexes.Contains(randomIndex));

            indexes.Add(randomIndex);
            powersChosen.Add(_powers[randomIndex]);
        }

        return powersChosen;
    }

    private void ChoosePower(Control power)
    {
        _player.StatsComponent.
        Hide();
    }

    private void PowerControlGuiInput(InputEvent @event, Control origin)
    {
        if (@event is InputEventMouseButton mouseInput)
        {
            if (mouseInput.ButtonIndex == MouseButton.Left && mouseInput.Pressed)
            {
                ChoosePower(origin);
            }
        }
    }
}
