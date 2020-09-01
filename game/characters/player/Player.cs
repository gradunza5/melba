using Godot;
using System;

namespace Melba
{
    public class Player : Character
    {
        [Export]
        public PackedScene Projectile;

        private Timer _invincibilityTimer;
        private bool _isInvincible = false;
        private Timer _attackCooldownTimer;
        private bool _canAttack = true;
        private Vector2 _velocity = Vector2.Zero;

        public override void _Ready()
        {
            base._Ready();
            _invincibilityTimer = GetNode<Timer>("InvincibilityTimer");
            _attackCooldownTimer = GetNode<Timer>("AttackCooldown");
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
            var direction = UserInputDirection();
            _velocity = direction * Speed;
            _velocity = MoveAndSlide(_velocity);
            PlayWalkAnimation(_velocity);
            ApplyDamageFromCollisions();
            PerformAttack();
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
                if (direction.y <= -velocity.UnitVectorSideLength()) { sprite.Play("up"); }
                else if (direction.x <= -velocity.UnitVectorSideLength()) { sprite.Play("left"); }
                else if (direction.x >= velocity.UnitVectorSideLength()) { sprite.Play("right"); }
                else if (direction.y >= velocity.UnitVectorSideLength()) { sprite.Play("down"); }
            }
            else
            {
                sprite.Stop();
                sprite.Frame = 0;
            }
        }

        private void ApplyDamageFromCollisions()
        {
            if (_isInvincible) { return; }
            for (int i = 0; i < GetSlideCount(); ++i)
            {
                var collision = GetSlideCollision(i);
                var enemy = collision.Collider as Enemy;
                if (enemy != null)
                {
                    Damage(enemy.CollisionDamage);
                    _isInvincible = true;
                    _invincibilityTimer.Start();
                }
            }
        }

        private void PerformAttack()
        {
            if (!_canAttack) { return; }
            if (Input.IsActionPressed("ui_select"))
            {
                var arrow = (Arrow)Projectile.Instance();
                GetParent().AddChild(arrow);
                arrow.Start(this, GetLocalMousePosition());

                _canAttack = false;
                _attackCooldownTimer.Start();
            }
        }

        public void OnInvincibilityTimeout() { _isInvincible = false; }
        public void OnAttackCooldownTimeout() { _canAttack = true; }
    }
}
