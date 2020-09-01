using Godot;
using System;

namespace Melba
{
    public class Bubble : Enemy
    {
        [Export]
        public PackedScene PatrolPath;
        private Path2D _navigator = null;
        private PathFollow2D _patrol = null;
        private Player _spottedPlayer = null;

        public override void _Ready()
        {
            base._Ready();
            Connect(nameof(HealthChanged), this, nameof(OnHealthChanged));

            _navigator = (Path2D)PatrolPath.Instance();
            _navigator.GlobalPosition = GlobalPosition;
            _patrol = _navigator.GetNode<PathFollow2D>("PathFollow2D");
            GetParent().CallDeferred("add_child", _navigator);
            SetPhysicsProcess(false);
            CallDeferred("set_physics_process", true);
        }

        private void OnHealthChanged(int current, int maximum)
        {
            if (current == 0)
            {
                QueueFree();
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
            if (_spottedPlayer != null)
            {
                _navigator.GlobalPosition = GlobalPosition;
                var playerDirection = (_spottedPlayer.Position - Position).Normalized();
                MoveAndSlide(playerDirection * Speed);
            }
            else
            {
                _patrol.Offset += (Speed * delta);
                var patrolDirection = (_patrol.GlobalPosition - GlobalPosition).Normalized();
                MoveAndSlide(patrolDirection * Speed);
            }
        }

        public void OnDetectionRadiusEntered(Node body)
        {
            var player = body as Player;
            if (player != null)
            {
                _spottedPlayer = player;
            }
        }

        public void OnDetectionRadiusExited(Node body)
        {
            if (body is Player)
            {
                _spottedPlayer = null;
            }
        }
    }
}
