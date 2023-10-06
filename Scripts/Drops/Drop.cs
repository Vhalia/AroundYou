using AroundYou.Utils.Attributes;
using AroundYou.Utils.Extensions;
using Godot;

namespace AroundYou.Scripts
{
    public partial class Drop : Node2D
    {
        [Export]
        private bool _following = false;
        [Export]
        private float _speed = 150;

        [Node("FollowArea")]
        public Area2D followArea;

        public float PickupRange { get; set; }

        [Signal]
        public delegate void DropPickedEventHandler();

        public override void _Ready()
        {
            base._Ready();
            this.WireNodes();

            followArea.BodyShapeEntered += FollowArea_BodyShapeEntered;
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            if (_following)
            {
                Player player = this.GetMain().GetNode("Player") as Player;
                FollowTarget(player.GlobalPosition, delta);
            }
        }

        public void Init(Vector2 position)
        {
            GlobalPosition = position;
        }

        private void FollowTarget(Vector2 targetPosition, double delta)
        {
            var direction = (targetPosition - GlobalPosition).Normalized();

            //GlobalPosition += direction * _speed * (float)delta;

            GlobalPosition = new Vector2(
                (float)Mathf.Lerp(GlobalPosition.X, GlobalPosition.X + direction.X * _speed * delta, 0.8),
                (float)Mathf.Lerp(GlobalPosition.Y, GlobalPosition.Y + direction.Y * _speed * delta, 0.8));
        }

        private void FollowArea_BodyShapeEntered(Rid bodyRid, Node2D body, long bodyShapeIndex, long localShapeIndex)
        {
            if (body is Player)
            {
                _following = true;
            }
        }
    }
}
