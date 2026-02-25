using Godot;
using pong.Scripts.Gameplay;
using Pong.Scripts.Core;
using System;
namespace Pong.Scripts.Nodes;

public partial class BallNode : CharacterBody2D
{
    const float BaseSpeed = 300f;
    [Export]
    public float Speed;
    [Export]
    public bool CanMove = false;
    public event Action<bool> OnGoal;
    private Vector2 _direction;
    private float _screenHeight;
    private float _screenWidth;
    private float _radius;

    public override void _Ready()
    {
        Speed = BaseSpeed;
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
            ResetToCenter();
            OnGoal?.Invoke(false);
        }

        if (Position.X >= _screenWidth - _radius)
        {
            ResetToCenter();
            OnGoal?.Invoke(true);
        }
    }
    public void ResetToCenter()
    {
        GlobalPosition = GetViewportRect().Size / 2;
        Velocity = Vector2.Zero;
    }

    public void RandomizeDirection()
    {
        float dir = GD.Randf() > 0.5f ? 1 : -1;
        Velocity = new Vector2(400 * dir, GD.RandRange(-200, 200));
    }
    public void IncreaseSpeed(float amount)
    {
        Speed += amount;
    }
    public override void _PhysicsProcess(double delta)
    {
        if (!CanMove) return;
        Velocity = Speed * _direction;

        MoveAndSlide();

        var collision = GetLastSlideCollision();

        if (Position.Y <= (_radius + 70.0) || Position.Y >= _screenHeight - _radius)
        {
            _direction = _direction.Bounce(Vector2.Up);
        }
        else if (collision != null)
        {
            _direction = _direction.Bounce(collision.GetNormal());

        }
    }


}
