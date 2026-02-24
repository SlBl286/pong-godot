using Godot;
using Pong.Scripts.Nodes;

namespace Pong.Scripts.Core;

public partial class GameManager : Node
{
    [Export] private int startTime = 120;
    private int _leftScore = 0;
    private int _rightScore = 0;
    private Label _leftScoreLabel;
    private Label _rightScoreLabel;
    private int _timeLeft;
    private Label _timeLabel;
    private Timer _timer;
    private BallNode _ball;

    public override void _Ready()
    {
        _ball = GetNode<BallNode>("../Main/Ball");
        _timeLabel = GetNode<Label>("../Main/UI/TimeLabel");
        _timer = GetNode<Timer>("../Main/GameTimer");
        _leftScoreLabel = GetNode<Label>("../Main/UI/Player1ScoreBoard/Player1Score");
        _rightScoreLabel = GetNode<Label>("../Main/UI/Player2ScoreBoard/Player2Score");

        _timer.Timeout += OnTimerTimeout;
        StartGame();
        _ball.LeftGoal += ScoreRight;
        _ball.RightGoal += ScoreLeft;

    }
    private void StartGame()
    {
        _timeLeft = startTime;
        UpdateUI();
        _timer.Start();
    }
    private void OnTimerTimeout()
    {
        _timeLeft--;
        UpdateUI();

        if (_timeLeft <= 0)
        {
            _timer.Stop();
            GD.Print("Time Over");
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
        _leftScoreLabel.Text = $"{_leftScore}";
        _rightScoreLabel.Text = $"{_rightScore}";

    }
}