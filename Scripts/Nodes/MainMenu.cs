using Godot;
using pong.Scripts.Core;
using Pong.Scripts.Core;

namespace Pong.Scripts.Nodes;

public partial class MainMenu : Control
{
        private InGameMain _inGame;

    public override void _Ready()
    {
        GetNode<Button>("VBoxContainer/VsAIBtn").Pressed += OnVsAI;
        GetNode<Button>("VBoxContainer/PvPBtn").Pressed += OnVsPlayer;
    }

    private void OnVsAI()
    {
        GameSettings.Mode = GameMode.VsAI;
        GetTree().ChangeSceneToFile("res://Scenes//Main.tscn");
             _inGame = GetNode<InGameMain>("../Main");
        _inGame.OnGoToInGame += OnVsPlayer;
    }

    private void OnVsPlayer()
    {
        GameSettings.Mode = GameMode.VsPlayer;
        GetTree().ChangeSceneToFile("res://Scenes//Main.tscn");
    }
}
