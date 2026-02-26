using Godot;

namespace Pong.Scripts.Gameplay;

public class BallLogic
{
    private const float BaseSpeed = 300f;
    private const float MaxSpeed = 800f;
    private const float SpeedIncrement = 20f;

    public float Speed { get; set; }
    public Vector2 Direction { get; set; }

    public BallLogic()
    {
        Reset();
    }

    public void Reset()
    {
        Speed = BaseSpeed;
        RandomizeDirection();
    }

    public void RandomizeDirection()
    {
        float x = GD.Randf() > 0.5f ? 1 : -1;
        float y = GD.RandRange(-1, 1);
        Direction = new Vector2(x, y).Normalized();
    }

    public Vector2 GetVelocity()
    {
        return Direction * Speed;
    }

    public void BounceVertical()
    {
        Direction = new Vector2(Direction.X, -Direction.Y).Normalized();
    }

    public void BounceOffNormal(Vector2 normal)
    {
        Direction = Direction.Bounce(normal).Normalized();
    }

    public void IncreaseSpeed()
    {
        Speed = Mathf.Min(Speed + SpeedIncrement, MaxSpeed);
    }

    public void IncreaseSpeed(float amount)
    {
        Speed = Mathf.Min(Speed + amount, MaxSpeed);
    }
}
