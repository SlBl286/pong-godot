using Godot;
using Pong.Scripts.Core;
namespace Pong.Scripts.Nodes;
public partial class RightGoal : Area2D
{
    private void OnBodyEntered(Node body)
{
    if (body is BallNode)
    {
        
    }
}
}
