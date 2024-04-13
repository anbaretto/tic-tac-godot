using Godot;
using TicTacGodot.Utility;
using TicTacToe;

namespace TicTacGodot;

public partial class GameOverScreen : Control
{
    [Export] private Button _restartButton;
    [Export] private Button _exitButton;
    [Export] private Label _mathResultLabel;

    [Export(PropertyHint.File, FileType.Scene)]
    private string _initialScreenPath;

    private MatchService _matchService;

    public override void _Ready()
    {
        _matchService = GetNode<MatchService>(AutoloadPath.MatchService);

        UpdateResultLabel();

        _restartButton.Pressed += OnRestartButtonPressed;
        _exitButton.Pressed += OnExitButtonPressed;
    }

    private void UpdateResultLabel()
    {
        var matchState = _matchService.CurrentState;

        switch (matchState)
        {
            case TieState:
                _mathResultLabel.Text = "It's a tie!";
                break;

            case PlayerWonState playerWonState:
                _mathResultLabel.Text = $"Player {playerWonState.Winner} won!";
                break;

            default:
                GD.PushError($"Unexpected match state at GameOverWindow: {matchState}");
                break;
        }
    }

    private void OnRestartButtonPressed() => GetTree().ReloadCurrentScene();

    private void OnExitButtonPressed() => GetTree().ChangeSceneToFile(_initialScreenPath);
}