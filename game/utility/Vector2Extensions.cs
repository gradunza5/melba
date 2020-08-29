using Godot;
using System;

namespace Godot 
{
    public static class Vector2Extensions
    {
        public static Vector2 RotatedRandomly(this Vector2 self)
        {
            var rotation = (float)(new Random().NextDouble() * 2 * Mathf.Pi);
            return self.Rotated(rotation);
        }
    }
}
