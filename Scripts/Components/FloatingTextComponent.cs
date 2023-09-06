using AroundYou.Utils.Extensions;
using Godot;

public partial class FloatingTextComponent : Node2D
{
    [Export]
    public PackedScene FloatingTextEffect;

    public override void _Ready()
    {
        this.WireNodes();
    }

    public void DisplayDamageNumber(float amount)
    {
        if (FloatingTextEffect == null)
        {
            return;
        }

        Effect floatingTextEffectInstance = FloatingTextEffect.Instantiate<Effect>();
        floatingTextEffectInstance.Init(Owner as Node2D, "floatingTextAnim", () =>
        {
            Label label = floatingTextEffectInstance.GetNode<Label>("Label");
            label.Hide();
            label.Text = amount.ToString();
        });
    }

}
