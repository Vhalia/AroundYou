using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;

namespace AroundYou.Scripts.States.CharacterStates;
public partial class MovingState : State
{
    [Node("MoveComponent")]
    private MoveComponent MoveComponent;
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
        AnimationTree.Set("parameters/conditions/isMoving", true);
    }

    public override void OnExitState()
    {
        AnimationTree.Set("parameters/conditions/isMoving", false);

    }

    public override void Update(double delta)
    {
    }

    public override void FixedUpdate(double delta)
    {
        MoveComponent.Move(Character.Direction, delta);
        if (Mathf.Abs(Character.Velocity.X) <= 0.5f && Mathf.Abs(Character.Velocity.Y) <= 0.5f)
        {
            Character.StateMachine.ChangeState(GetNodeOrNull<State>("../IDLE"));
        }
    }
}
