using Godot;
using Pong.Scripts.Nodes;

namespace Pong.Scripts.Core;

public partial class GameManager : Node
{
    [Export] private int gameTime = 60;
    private int _timeToStart = 3;
    private int _leftScore = 0;
    private int _rightScore = 0;
    private Label _leftScoreLabel;
    private Label _rightScoreLabel;
    private int _timeLeft;
    private Label _timeLabel;
    private Label _timeToStartLabel;

    private Timer _gameTimer;
    private Timer _startTimer;

    private BallNode _ball;
    private Panel _gameOverPanel;

    public override void _Ready()
    {
        _ball = GetNode<BallNode>("../Main/Ball");
        _timeLabel = GetNode<Label>("../Main/UI/TimeLabel");
        _gameOverPanel = GetNode<Panel>("../Main/UI/GameOverPanel");
        _gameTimer = GetNode<Timer>("../Main/GameTimer");
        _leftScoreLabel = GetNode<Label>("../Main/UI/Player1ScoreBoard/Player1Score");
        _rightScoreLabel = GetNode<Label>("../Main/UI/Player2ScoreBoard/Player2Score");
        _startTimer = GetNode<Timer>("../Main/PrepareTimer");
        _timeToStartLabel = GetNode<Label>("../Main/UI/CountToStartLabel");
        _startTimer.Timeout += OnStartTimerTimeout;
        _gameTimer.Timeout += OnGameTimerTimeout;
        PrepareGame();
        _ball.LeftGoal += ScoreRight;
        _ball.RightGoal += ScoreLeft;

    }
    private void PrepareGame()
    {
        UpdateUI();
        _startTimer.Start();
    }
    private void StartGame()
    {
        _timeToStartLabel.Visible = false;
        _ball.Visible = true;
        _timeLeft = gameTime;
        UpdateUI();
        _gameTimer.Start();
    }
    private void OnStartTimerTimeout()
    {
        _timeToStart--;
        UpdateUI();

        if (_timeToStart <= 0)
        {
            _startTimer.Stop(); 
            StartGame();
        }
    }
    private void OnGameTimerTimeout()
    {
        _timeLeft--;
        UpdateUI();

        if (_timeLeft <= 0)
        {
            _gameTimer.Stop();
            _gameOverPanel.Visible = true;
            GetTree().Paused = true;
        }
    }
    public void ScoreLeft()
    {

        _leftScore++;
        UpdateUI();
    }

    public void ScoreRight()
    {
        GD.Print("Player scored!");

        _rightScore++;
        UpdateUI();
    }

    private void ResetBall()
    {
        var ball = GetNode<BallNode>("../Ball");
        ball.Position = Vector2.Zero;
    }
    private void UpdateUI()
    {


        int minutes = _timeLeft / 60;
        int seconds = _timeLeft % 60;

        _timeLabel.Text = $"{minutes:00}:{seconds:00}";
        _timeToStartLabel.Text = $"{_timeToStart}";

        _leftScoreLabel.Text = $"{_leftScore}";
        _rightScoreLabel.Text = $"{_rightScore}";

    }
}