using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;
using System;

public partial class FloatingTextComponent : Node2D
{
    [Export]
    public PackedScene FloatingTextEffect;

    public override void _Ready()
    {
        this.WireNodes();
    }

    public void DisplayDamageNumber(int amount)
    {
        if (FloatingTextEffect == null)
            return;

        var floatingTextEffectInstance = FloatingTextEffect.Instantiate<Effect>();
        floatingTextEffectInstance.Init(Owner as Node2D, "floatingTextAnim", () =>
        {
            var label = floatingTextEffectInstance.GetNode<Label>("Label");
            label.Hide();
            label.Text = amount.ToString();
        });
    }

}
