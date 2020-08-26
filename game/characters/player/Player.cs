using Godot;
using System;

public class Player : Character
{
    [Export]
    public float InvincibilitySeconds;

    private float _timeUntilNextHit = 0;
    private Vector2 _velocity = Vector2.Zero;

    public override void _PhysicsProcess(float delta)
    {
        _velocity = UserInputVelocity();
        _velocity = MoveAndSlide(_velocity);
        ApplyDamageFromCollisions(delta);
    }

    private Vector2 UserInputVelocity()
    {
        var velocity = Vector2.Zero;
        if (Input.IsActionPressed("ui_left"))
            velocity.x -= 1;
        if (Input.IsActionPressed("ui_right"))
            velocity.x += 1;
        if (Input.IsActionPressed("ui_up"))
            velocity.y -= 1;
        if (Input.IsActionPressed("ui_down"))
            velocity.y += 1;
        return velocity.Normalized() * Speed;
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
