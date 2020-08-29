using Godot;
using System;

namespace Melba
{
    public class Bubble : Enemy
    {
        private Player _spottedPlayer = null;

        public override void _PhysicsProcess(float delta)
        {
            if (_spottedPlayer != null)
            {
                var playerDirection = (_spottedPlayer.Position - Position).Normalized();
                MoveAndSlide(playerDirection * Speed);
            }
            else
            {
                var randomDirection = Vector2.One.RotatedRandomly();
                MoveAndSlide(randomDirection * Speed);
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
