using Godot;
using System;

namespace Melba
{
    public class Hud : CanvasLayer
    {
        private TextureProgress _lifeBar;

        /// Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _lifeBar = GetNode<TextureProgress>("LifeBar");
        }

        public void SetHealth(int current, int maximum)
        {
            _lifeBar.MaxValue = maximum;
            _lifeBar.Value = current;
        }
    }
}
