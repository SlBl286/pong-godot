using Godot;

namespace Pong.Scripts.Nodes;

public partial class PaddleNode : CharacterBody2D
{
    private const float TopBoundary = 72f;

    [Export] public float Speed = 400;
    [Export] public bool CanMove = false;
    [Export] public string UpAction = "ui_up";
    [Export] public string DownAction = "ui_down";

    private float _halfHeight;
    private float _screenHeight;

    public override void _Ready()
    {
        _screenHeight = GetViewportRect().Size.Y;
        var shape = GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;
        _halfHeight = shape.Size.Y / 2;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!CanMove) return;

        float direction = 0;

        if (Input.IsActionPressed(UpAction))
            direction -= 1;
        if (Input.IsActionPressed(DownAction))
            direction += 1;

        Position += new Vector2(0, direction * Speed * (float)delta);
        Position = new Vector2(
            Position.X,
            Mathf.Clamp(Position.Y, _halfHeight + TopBoundary, _screenHeight - _halfHeight)
        );
    }
}
