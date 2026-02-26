using System;
using Godot;
using Pong.Scripts.Gameplay;

namespace Pong.Scripts.Nodes;

public partial class BallNode : CharacterBody2D
{
    private const float TopBoundary = 70f;

    [Export]
    public bool CanMove { get; set; } = false;

    public event Action<bool> OnGoalScored;

    private readonly BallLogic _logic = new();
    private float _radius;
    private float _screenHeight;
    private float _screenWidth;

    public Vector2 Direction
    {
        get => _logic.Direction;
        set => _logic.Direction = value;
    }

    public float Speed
    {
        get => _logic.Speed;
        set => _logic.Speed = value;
    }

    public override void _Ready()
    {
        _screenHeight = GetViewportRect().Size.Y;
        _screenWidth = GetViewportRect().Size.X;

        var shape = GetNode<CollisionShape2D>("CollisionShape2D").Shape as CircleShape2D;
        _radius = shape.Radius;
    }

    public override void _Process(double delta)
    {
        CheckGoal();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!CanMove) return;

        Velocity = _logic.GetVelocity();
        MoveAndSlide();
        HandleCollisions();
    }

    private void CheckGoal()
    {
        if (GlobalPosition.X - _radius <= 0)
        {
            Reset();
            OnGoalScored?.Invoke(false);
        }
        else if (GlobalPosition.X >= _screenWidth - _radius)
        {
            Reset();
            OnGoalScored?.Invoke(true);
        }
    }

    private void HandleCollisions()
    {
        bool hitTopOrBottom = Position.Y <= _radius + TopBoundary || 
                              Position.Y >= _screenHeight - _radius;

        if (hitTopOrBottom)
        {
            _logic.BounceVertical();
            Position = new Vector2(Position.X, Mathf.Clamp(Position.Y, _radius + TopBoundary + 1, _screenHeight - _radius - 1));
        }

        var collision = GetLastSlideCollision();
        if (collision != null)
        {
            _logic.BounceOffNormal(collision.GetNormal());
            _logic.IncreaseSpeed();
        }
    }

    public void Reset()
    {
        GlobalPosition = GetViewportRect().Size / 2;
        Velocity = Vector2.Zero;
        _logic.Reset();
    }

    public void Start()
    {
        Reset();
        CanMove = true;
    }

    public void Stop()
    {
        CanMove = false;
        Velocity = Vector2.Zero;
    }

    public void RandomizeDirection() => _logic.RandomizeDirection();

    public void IncreaseSpeed(float amount) => _logic.IncreaseSpeed(amount);
}
