namespace Pong.Scripts.Core;

public enum GameMode
{
    VsAI,
    VsPlayer
}
public enum GameScene
{
    MainMenu,
    InGame
}

public static class GameSettings
{
    public static GameMode Mode = GameMode.VsPlayer;
    public static GameScene CurrentScene = GameScene.MainMenu;
}