using Godot;
using System;

namespace Melba
{
    public class Player : Character
    {
        [Export]
        public float InvincibilitySeconds;

        private float _timeUntilNextHit = 0;
        private Vector2 _velocity = Vector2.Zero;
        private const float UNIT_VECTOR_SIDE_LEN = 0.7071068f;

        public override void _PhysicsProcess(float delta)
        {
            var direction = UserInputDirection();
            _velocity = direction * Speed;
            _velocity = MoveAndSlide(_velocity);
            PlayWalkAnimation(_velocity);
            ApplyDamageFromCollisions(delta);
        }

        private static Vector2 UserInputDirection()
        {
            var direction = Vector2.Zero;
            if (Input.IsActionPressed("ui_left")) { direction.x -= 1; }
            if (Input.IsActionPressed("ui_right")) { direction.x += 1; }
            if (Input.IsActionPressed("ui_up")) { direction.y -= 1; }
            if (Input.IsActionPressed("ui_down")) { direction.y += 1; }
            return direction = direction.Length() > 0 ? direction.Normalized() : direction;
        }

        private void PlayWalkAnimation(Vector2 velocity)
        {
            var sprite = GetNode<AnimatedSprite>("AnimatedSprite");
            if (velocity != Vector2.Zero)
            {
                var direction = velocity.Normalized();
                if (direction.y <= -UNIT_VECTOR_SIDE_LEN) { sprite.Play("up"); }
                else if (direction.x <= -UNIT_VECTOR_SIDE_LEN) { sprite.Play("left"); }
                else if (direction.x >= UNIT_VECTOR_SIDE_LEN) { sprite.Play("right"); }
                else { sprite.Play("down"); }
            }
            else
            {
                sprite.Stop();
                sprite.Frame = 0;
            }
        }

        private void ApplyDamageFromCollisions(float delta)
        {
            _timeUntilNextHit -= delta;
            for (int i = 0; i < GetSlideCount(); ++i)
            {
                var collision = GetSlideCollision(i);
                var enemy = collision.Collider as Enemy;
                if ((enemy != null) && (_timeUntilNextHit <= 0))
                {
                    Damage(enemy.CollisionDamage);
                    _timeUntilNextHit = InvincibilitySeconds;
                }
            }
        }
    }
}
