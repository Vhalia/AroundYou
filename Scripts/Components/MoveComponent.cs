using Godot;

public partial class MoveComponent : Node2D
{
    [Export]
    public StatsComponent StatsComponent;
    [Export]
    public float Acceleration = 0.5f;
    [Export]
    public float Desceleration = 0.5f;

    public CharacterBody2D CharacterBody;

    public override void _Ready()
    {
        CharacterBody = Owner as CharacterBody2D;
    }

    public void Move(Vector2 direction, double delta)
    {
        CharacterBody.Velocity = direction != Vector2.Zero
            ? new Vector2(
                (float)Mathf.Lerp(CharacterBody.Velocity.X, direction.X * StatsComponent.MovementSpeed * delta, Acceleration),
                (float)Mathf.Lerp(CharacterBody.Velocity.Y, direction.Y * StatsComponent.MovementSpeed * delta, Acceleration)
            )
            : new Vector2(
                (float)Mathf.Lerp(CharacterBody.Velocity.X, 0, Desceleration),
                (float)Mathf.Lerp(CharacterBody.Velocity.Y, 0, Desceleration)
            );

        _ = CharacterBody.MoveAndSlide();
    }
}
