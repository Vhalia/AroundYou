using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;
using System;

public partial class ChoosePowerUI : Control
{
    [Node("MarginController/HBoxContainer/Power1")]
    private Control _power1;
    [Node("MarginController/HBoxContainer/Power2")]
    private Control _power2;
    [Node("MarginController/HBoxContainer/Power3")]
    private Control _power3;

    private 

    public override void _Ready()
    {
        base._Ready();
        this.WireNodes();

        _power1.GuiInput += (@event) => PowerControlGuiInput(@event, _power1);
        _power2.GuiInput += (@event) => PowerControlGuiInput(@event, _power2);
        _power3.GuiInput += (@event) => PowerControlGuiInput(@event, _power3);
    }

    public void Init()
    {

    }

    private void ChoosePower(Control power)
    {
        this.Hide();
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
