using Godot;

namespace AroundYou.Scripts.States
{
    public abstract partial class State : Node2D
    {
        public abstract void OnEnterState();
        public abstract void OnExitState();
        public abstract void Update(double delta);
        public abstract void FixedUpdate(double delta);
    }
}
