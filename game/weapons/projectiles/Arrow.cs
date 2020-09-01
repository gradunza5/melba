using Godot;
using System;

namespace Melba
{
    public class Arrow : Area2D
    {
        [Export]
        public int Speed { get; set; }
        [Export]
        public int Damage { get; set; }
        [Export]
        public float Cooldown { get; set; }

        private Vector2 _velocity = Vector2.Zero;
        private Node2D _source = null;

        public void Start(Node2D source, Vector2 direction)
        {
            _source = source;
            GlobalPosition = _source.GlobalPosition;
            _velocity = direction.Normalized() * Speed;
            Rotate(-direction.AngleTo(Vector2.One));
            GetNode<Timer>("ExpiryTimer").Start();
        }

        void OnBodyEntered(Node body)
        {
            if (body != _source)
            {
                var character = body as Character;
                if (character != null)
                {
                    character.Damage(Damage);
                }
                QueueFree();
            }
        }

        void OnLifetimeTimeout()
        {
            QueueFree();
        }

        /// Called during the physics processing step of the main loop. @delta elapsed time since the previous frame
        public override void _PhysicsProcess(float delta)
        {
            Position += _velocity * delta;
        }
    }
}
