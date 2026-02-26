using System;
using Godot;
using Pong.Scripts.Core;

namespace Pong.Scripts.Nodes;

public partial class InGameMain : Node2D
{
    private const int PrepareTime = 3;
    private const float AiSpeed = 350f;
    private const float AiDeadZone = 10f;
    private const float TopBoundary = 112f;
    private const float BottomBoundary = 622f;

    [Export] public int WinningScore = 5;
    [Export] public int GameTime = 120;

    private BallNode _ball;
    private PaddleNode _leftPaddle;
    private PaddleNode _rightPaddle;
    private CanvasLayer _ui;
    private Timer _prepareTimer;
    private Timer _gameTimer;
    private Label _timeLabel;
    private Label _prepareTimeLabel;
    private Panel _gameOverPanel;
    private int _leftScore;
    private int _rightScore;
    private int _timeLeft;
    private int _prepareTimeLeft;
    private bool _isGameRunning;
    private GameMode _gameMode;

    public override void _Ready()
    {
        _gameMode = GameSettings.Mode;

        _ball = GetNode<BallNode>("Ball");
        _leftPaddle = GetNode<PaddleNode>("LeftPaddle");
        _rightPaddle = GetNode<PaddleNode>("RightPaddle");
        _ui = GetNode<CanvasLayer>("UI");
        _prepareTimer = GetNode<Timer>("PrepareTimer");
        _gameTimer = GetNode<Timer>("GameTimer");
        _timeLabel = GetNode<Label>("UI/TimeLabel");
        _prepareTimeLabel = GetNode<Label>("UI/PrepareTimeLabel");
        _gameOverPanel = GetNode<Panel>("UI/GameOverPanel");

        _ball.OnGoalScored += OnGoalScored;
        _gameTimer.Timeout += OnGameTimerTick;
        _prepareTimer.Timeout += OnPrepareTimerTick;


        SetupGameMode();
        StartPreparePhase();
    }

    private void SetupGameMode()
    {
        _leftPaddle.UpAction = "p1_up";
        _leftPaddle.DownAction = "p1_down";
        if (_gameMode == GameMode.VsAI)
        {
            _rightPaddle.SetProcess(false);
            _rightPaddle.SetPhysicsProcess(false);
        }
        else
        {
            _rightPaddle.UpAction = "p2_up";
            _rightPaddle.DownAction = "p2_down";
            _rightPaddle.SetProcess(true);
            _rightPaddle.SetPhysicsProcess(true);
        }
    }

    private void StartPreparePhase()
    {
        _isGameRunning = false;
        _prepareTimeLeft = PrepareTime;
        _ball.Stop();
        _leftPaddle.CanMove = false;
        _rightPaddle.CanMove = false;
        _prepareTimeLabel.Visible = true;
        _ball.Visible = false;
        _prepareTimer.Start();

    }

    private void OnPrepareTimerTick()
    {
        UpdateTimeUI();
        _prepareTimeLeft--;
        if (_prepareTimeLeft <= 0)
        {
            _prepareTimeLabel.Visible = false;
            _prepareTimer.Stop();
            StartGame();
        }
    }

    private void StartGame()
    {
        _isGameRunning = true;
        _timeLeft = GameTime;
        _leftPaddle.CanMove = true;
        _rightPaddle.CanMove = true;
        _ball.Visible = true;
        _ball.Start();
        _gameTimer.Start();
        UpdateTimeUI();
    }

    private void OnGameTimerTick()
    {
        _timeLeft--;
        UpdateTimeUI();

        if (_timeLeft <= 0)
        {
            _gameTimer.Stop();
            EndGame();
        }
    }

    private void OnGoalScored(bool isRightGoal)
    {
        if (isRightGoal)
            _rightScore++;
        else
            _leftScore++;

        UpdateScoreUI();

        if (_leftScore >= WinningScore || _rightScore >= WinningScore)
            EndGame();
        else
            StartPreparePhase();
    }

    private void UpdateScoreUI()
    {
        var leftLabel = _ui.GetNode<Label>("VBoxContainer/LeftScore");
        var rightLabel = _ui.GetNode<Label>("VBoxContainer/RightScore");

        if (leftLabel != null)
            leftLabel.Text = _leftScore.ToString();
        if (rightLabel != null)
            rightLabel.Text = _rightScore.ToString();
    }

    private void UpdateTimeUI()
    {
        if (_timeLabel != null && !_gameTimer.IsStopped())
        {
            int minutes = _timeLeft / 60;
            int seconds = _timeLeft % 60;
            _timeLabel.Text = $"{minutes:00}:{seconds:00}";
        }
        if (_prepareTimeLabel != null && !_prepareTimer.IsStopped())
        {
            _prepareTimeLabel.Scale = Vector2.One * 2;
            var tween = CreateTween();
            tween.TweenProperty(
                _prepareTimeLabel,
                "scale",
                Vector2.One,
                0.4f
            ).SetTrans(Tween.TransitionType.Back)
             .SetEase(Tween.EaseType.Out);
            _prepareTimeLabel.Text = $"{_prepareTimeLeft}";
        }
    }

    private void EndGame()
    {
        _isGameRunning = false;
        _ball.Stop();
        _leftPaddle.CanMove = false;
        _rightPaddle.CanMove = false;
        _gameTimer.Stop();

        if (_gameOverPanel != null)
            _gameOverPanel.Visible = true;

        string winner = _leftScore >= WinningScore ? "Left" : "Right";
        GD.Print($"Game Over! {winner} player wins!");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_gameMode == GameMode.VsAI && _isGameRunning)
            UpdateAI(delta);
    }

    private void UpdateAI(double delta)
    {
        float targetY = _ball.GlobalPosition.Y;
        float currentY = _rightPaddle.GlobalPosition.Y;
        float diff = targetY - currentY;

        if (Mathf.Abs(diff) > AiDeadZone)
        {
            float direction = diff > 0 ? 1 : -1;
            _rightPaddle.Position += new Vector2(0, direction * AiSpeed * (float)delta);
            _rightPaddle.Position = new Vector2(_rightPaddle.Position.X, Mathf.Clamp(_rightPaddle.Position.Y, TopBoundary, BottomBoundary));
        }
    }
}
