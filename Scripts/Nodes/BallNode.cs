using Godot;
using pong.Scripts.Gameplay;
using Pong.Scripts.Core;
using System;
namespace Pong.Scripts.Nodes;

public partial class BallNode : CharacterBody2D
{
    [Export]
    public float Speed = 300;
    public event Action LeftGoal;
    public event Action RightGoal;
    private BallLogic _logic;
    private Vector2 _direction;
    private float _screenHeight;
    private float _screenWidth;
    private float _radius;

    public override void _Ready()
    {
        _logic = new BallLogic();

        _direction = new Vector2(1, 0).Normalized();
        _screenHeight = GetViewportRect().Size.Y;
        _screenWidth = GetViewportRect().Size.X;

        var shape = GetNode<CollisionShape2D>("CollisionShape2D").Shape as CircleShape2D;
        _radius = shape.Radius;
    }
    public override void _Process(double delta)
    {
        if (GlobalPosition.X - _radius <= 0)
        {
            Position = new Vector2(640, 483);
            LeftGoal?.Invoke();

        }

        if (Position.X >= _screenWidth - _radius)
        {
            Position = new Vector2(640, 483);
            RightGoal?.Invoke();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = _logic.GetVelocity();

        MoveAndSlide();

        var collision = GetLastSlideCollision();

        if (Position.Y <= (_radius + 70.0) || Position.Y >= _screenHeight - _radius)
        {
            _logic.Bounce(Vector2.Up);
        }
        else if (collision != null)
        {
            _logic.Bounce(collision.GetNormal());
        }
    }


}
