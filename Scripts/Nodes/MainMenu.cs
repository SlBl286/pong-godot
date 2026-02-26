using Godot;
using Pong.Scripts.Core;

namespace Pong.Scripts.Nodes;

public partial class MainMenu : Control
{
    public override void _Ready()
    {
        GetNode<Button>("VBoxContainer/VsAIBtn").Pressed += OnVsAI;
        GetNode<Button>("VBoxContainer/PvPBtn").Pressed += OnVsPlayer;
    }

    private void OnVsAI()
    {
        GameSettings.Mode = GameMode.VsAI;
        GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
    }

    private void OnVsPlayer()
    {
        GameSettings.Mode = GameMode.VsPlayer;
        GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
    }
}
