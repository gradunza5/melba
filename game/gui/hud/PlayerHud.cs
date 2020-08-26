using Godot;
using System;

public class PlayerHud : Control
{
    private ColorRect _fullBar;
    private Vector2 _initialSize = Vector2.Zero;

    /// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _fullBar = GetNode<ColorRect>("LifeBar/FullBar");
        _initialSize = _fullBar.RectSize;
    }

    public void SetHealth(int current, int maximum)
    {
        _fullBar.RectSize = new Vector2(
            x: _initialSize.x * Math.Max((float)current / maximum, 0),
            y: _initialSize.y
        );
    }
}
