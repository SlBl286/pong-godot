using Godot;
namespace Pong.Scripts.Nodes;

public partial class PaddleNode : CharacterBody2D
{
    [Export]
    public float Speed = 400;
    private float _halfHeight;
    private float _screenHeight;
    public override void _Ready()
    {
        _screenHeight = GetViewportRect().Size.Y;

        // Lấy chiều cao paddle từ CollisionShape
        var shape = GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;
        _halfHeight = shape.Size.Y / 2;
    }
    public override void _PhysicsProcess(double delta)
    {
        float direction = 0;

        if (Input.IsActionPressed("move_up"))
        {
            direction -= 1;
        }
        if (Input.IsActionPressed("move_down"))
        {
            direction += 1;
        }

        Position += new Vector2(0, direction * 400f * (float)delta);

        // Clamp giới hạn trên và dưới
        Position = new Vector2(
            Position.X,
            Mathf.Clamp(
                Position.Y,
                _halfHeight,
                _screenHeight - _halfHeight
            )
        );
    }

}
