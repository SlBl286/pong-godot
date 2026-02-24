using System;
using Godot;

namespace pong.Scripts.Gameplay
{
    public class BallLogic
    {
        public float Speed { get; set; } = 300f;
        public Vector2 Direction { get; private set; }

        public BallLogic()
        {
            Reset();
        }

        public void Reset()
        {
            
            var rand = new Random();
            float x = rand.Next(0, 2) == 0 ? -1 : 1;
            float y = (float)(rand.NextDouble() * 2 - 1);
            Direction = new Vector2(x, y).Normalized();

        }

        public Vector2 GetVelocity()
        {
            return Direction * Speed;
        }

        public void Bounce(Vector2 normal)
        {
            Direction = Direction.Bounce(normal);
        }
    }
}