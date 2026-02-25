using System;
using Godot;

namespace Pong.Scripts.Nodes;

public partial class InGameMain : Node2D
{
    public event Action OnGoToInGame;

    public override void _Ready()
    {
        OnGoToInGame?.Invoke();
    }
}
