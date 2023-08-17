using AroundYou.Scripts;
using Godot;

public partial class Bullet : Area2D
{
    public Vector2 Direction = Vector2.Zero;
    public float Speed;
    public int Damage;

    public override void _Ready()
    {
        BodyShapeEntered += Bullet_BodyShapeEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Direction != Vector2.Zero)
        {
            GlobalPosition += Speed * ((float)delta) * Direction;
        }
    }

    public void Init(int damage, float speed, Vector2 direction, Vector2 spawn)
    {
        GlobalPosition = spawn;
        Damage = damage;
        Speed = speed;
        Direction = direction;
    }

    private void Bullet_BodyShapeEntered(Rid bodyRid, Node2D body, long bodyShapeIndex, long localShapeIndex)
    {
        if (body is Character)
        {
            (body as Character)?.TakeDamage(Damage);
        }
        QueueFree();
    }
}
