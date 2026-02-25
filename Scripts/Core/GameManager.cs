using System.Threading.Tasks;
using Godot;
using pong.Scripts.Core;
using Pong.Scripts.Nodes;

namespace Pong.Scripts.Core;

public partial class GameManager : Node
{
    [Export] private int gameTime = 120;
    public float SpeedIncreaseInterval = 10f;
    public float SpeedIncreaseAmount = 50f;
    private bool _isRoundStarting = false;
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
    private PaddleNode _playerPaddle1;
    private PaddleNode _playerPaddle2;
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
        _playerPaddle1 = GetNode<PaddleNode>("../Main/LeftPaddle");
        _playerPaddle2 = GetNode<PaddleNode>("../Main/RightPaddle");
        _playerPaddle1.UpAction = "p1_up";
        _playerPaddle1.DownAction = "p1_down";
        _playerPaddle2.UpAction = "p2_up";
        _playerPaddle2.DownAction = "p2_down";
        _startTimer.Timeout += OnStartTimerTimeout;
        _gameTimer.Timeout += OnGameTimerTimeout;

        StartCountdown();

        _ball.OnGoal += OnGoal;

    }
    private void StartCountdown()
    {
        _timeToStartLabel.Visible = true;
        FreezeMovement();
        UpdateUI();
        _startTimer.Start();
    }
    private void StartGame()
    {

        EnableMovement();
        _timeToStartLabel.Visible = false;
        if (_timeLeft <= 0)
            _timeLeft = gameTime;
        UpdateUI();
        _gameTimer.Paused = false;
        _gameTimer.Start();
    }
    private async void OnStartTimerTimeout()
    {
        _timeToStart--;
        AnimateCountdown();
        UpdateUI();
        if (_timeToStart <= 0)
        {
            await SlowMotionEffect();
            _startTimer.Stop();
            StartGame();
        }
    }
    private void OnGameTimerTimeout()
    {
        _timeLeft--;
        UpdateUI();
        if (_timeLeft % SpeedIncreaseInterval == 0)
        {
            _ball.IncreaseSpeed(SpeedIncreaseAmount);

        }
        if (_timeLeft <= 0)
        {
            _gameTimer.Stop();
            _gameOverPanel.Visible = true;
            FreezeMovement();
        }
    }


    public void AddScore(bool player1Scored)
    {
        if (player1Scored)
            _leftScore++;
        else _rightScore++;
        UpdateUI();
    }
    private void OnGoal(bool player1Scored)
    {
        if (_isRoundStarting) return;
        _timeToStart = 3;
        _isRoundStarting = true;
        _gameTimer.Paused = true;
        AddScore(player1Scored);

        _ball.ResetToCenter();

        StartCountdown();
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

    private void AnimateCountdown()
    {
        _timeToStartLabel.Scale = Vector2.One * 2;

        var tween = CreateTween();
        tween.TweenProperty(
            _timeToStartLabel,
            "scale",
            Vector2.One,
            0.4f
        ).SetTrans(Tween.TransitionType.Back)
         .SetEase(Tween.EaseType.Out);
    }
    private void FreezeMovement()
    {
        _ball.Visible = false;
        _ball.CanMove = false;
        _playerPaddle1.CanMove = false;
        _playerPaddle2.CanMove = false;
    }
    private void EnableMovement()
    {
        _ball.RandomizeDirection();
        _ball.CanMove = true;
        _ball.Visible = true;
        _playerPaddle2.CanMove = true;
        _playerPaddle1.CanMove = true;

        _isRoundStarting = false;
    }
    private async Task SlowMotionEffect()
    {

        Engine.TimeScale = 0.4f; // chậm 5 lần

        await ToSignal(GetTree().CreateTimer(0.5f, true), "timeout");

        Engine.TimeScale = 1f;

    }
}