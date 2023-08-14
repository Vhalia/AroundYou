using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;

namespace AroundYou.Scripts.States.CharacterStates;
public partial class IdleState : State
{
    [Node("../../AnimationTree")]
    private AnimationTree AnimationTree;

    private Character Character;

    public override void _Ready()
    {
        this.WireNodes();
    }

    public override void OnEnterState()
    {
        Character = Owner as Character;
        AnimationTree.Set("parameters/conditions/isIdle", true);
    }

    public override void OnExitState()
    {
        AnimationTree.Set("parameters/conditions/isIdle", false);
    }

    public override void Update(double delta)
    {

    }

    public override void FixedUpdate(double delta)
    {
        if (Character.Direction != Vector2.Zero)
        {
            Character.StateMachine.ChangeState(GetNodeOrNull<State>("../MOVING"));
        }
    }
}
