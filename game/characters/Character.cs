using Godot;
using System;

namespace Melba
{
    public class Character : KinematicBody2D
    {
        [Signal]
        public delegate void HealthChanged(int current, int maximum);

        /// Maximum hit points of the character
        [Export]
        public int HealthMax { get; set; } = 1;

        /// Hit points of the character, emits HealthChanged when modified
        [Export]
        public int Health
        {
            get { return _health; }
            private set
            {
                if (_health != value)
                {
                    _health = value;
                    EmitSignal(nameof(HealthChanged), Health, HealthMax);
                }
            }
        }
        private int _health = 0; ///< Leave at 0 to initialize to HealthMax on _Ready

        /// "Relative movement speed of the character"
        [Export]
        public int Speed { get; set; } = 50;

        public override void _Ready()
        {
            if (_health == 0)
            {
                _health = HealthMax;
            }
        }

        /// Reduces Health down to a minimum of 0
        public void Damage(int points)
        {
            Health = Math.Max(Health - points, 0);
        }

        /// Restores Health up to a maximum of HealthMax
        public void Heal(int points)
        {
            OverHeal(points, HealthMax);
        }

        /// Allows healing in excess of player maximum hp, up to an optional maximum
        public void OverHeal(int points, int maximum = int.MaxValue)
        {
            Health = Math.Min(Health + points, maximum);
        }
    }
}
