using AroundYou.Scripts;
using Godot;
using System;

public partial class MoveComponent : Node2D
{
    [Export]
    public float Speed = 30;
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
        if (direction != Vector2.Zero)
        {
            CharacterBody.Velocity = new Vector2(
                (float)Mathf.Lerp(CharacterBody.Velocity.X, direction.X * Speed * delta, Acceleration),
                (float)Mathf.Lerp(CharacterBody.Velocity.Y, direction.Y * Speed * delta, Acceleration)
            );
        }
        else
        {
            CharacterBody.Velocity = new Vector2(
                (float)Mathf.Lerp(CharacterBody.Velocity.X, 0, Desceleration),
                (float)Mathf.Lerp(CharacterBody.Velocity.Y, 0, Desceleration)
            );
        }

        CharacterBody.MoveAndSlide();
    }
}
