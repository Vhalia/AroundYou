using Godot;

namespace AroundYou.Scripts.States;

public partial class StateMachine : Node2D
{
    public State CurrentState { get; set; }
    public State PreviousState { get; set; }

    public void ChangeState(State newState)
    {
        if (newState == null)
        {
            return;
        }

        PreviousState = CurrentState;
        CurrentState = newState;

        PreviousState?.OnExitState();
        CurrentState.OnEnterState();
    }

    public override void _Process(double delta)
    {
        CurrentState?.Update(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState?.FixedUpdate(delta);
    }
}
