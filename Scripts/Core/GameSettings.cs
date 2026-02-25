namespace pong.Scripts.Core;

public enum GameMode
{
    VsAI,
    VsPlayer
}
public enum GameScence
{
    MainMenu,
    InGame
}

public static class GameSettings
{
    public static GameMode Mode = GameMode.VsPlayer;
    public static GameScence CurrentScence = GameScence.MainMenu;

}