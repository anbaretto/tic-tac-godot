using Godot;
using TicTacGodot.Utility;
using TicTacToe;

namespace TicTacGodot;

public partial class TopBar : Control
{
	[ExportGroup("Scenes")]
	[Export(PropertyHint.File, FileType.Scene)]
	private string _initialScreenPath;

	[ExportGroup("UI")] 
	[Export] private Button _restartButton;
	[Export] private Button _exitButton;
	[Export] private Label _currentPlayerLabel;
	[Export] private Label _winRecordLabel;

	private MatchService _matchService;
	private ScoreCounter _scoreCounter;

	public override void _Ready()
	{
		_matchService = GetNode<MatchService>(AutoloadPath.MatchService);
		_scoreCounter = GetNode<ScoreCounter>(AutoloadPath.ScoreCounter);
		_matchService.StartMatch();

		_matchService.MatchStateChanged += OnMatchStateChanged;
		_scoreCounter.ScoreChanged += OnScoreChanged;
		_restartButton.Pressed += OnRestartButtonPressed;
		_exitButton.Pressed += OnExitButtonPressed;

		UpdatePlayerLabel(_matchService.CurrentState);
		UpdateWinRecordLabel(_scoreCounter.XScore, _scoreCounter.OScore);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_matchService.MatchStateChanged -= OnMatchStateChanged;
			_scoreCounter.ScoreChanged -= OnScoreChanged;
		}

		base.Dispose(disposing);
	}

	private void OnMatchStateChanged(MatchState state) => UpdatePlayerLabel(state);

	private void OnScoreChanged(int xScore, int oScore) => UpdateWinRecordLabel(xScore, oScore);

	private void OnExitButtonPressed() => ReturnToInitialScreen();

	private void OnRestartButtonPressed() => ReloadScene();

	private void UpdatePlayerLabel(MatchState state)
	{
		if (state is InProgressState inProgressState)
		{
			_currentPlayerLabel.Text = $"Current Player: {inProgressState.NextPlayer.ToString()}";
		}
		else
		{
			_currentPlayerLabel.Text = "Current Player: ?";
		}
	}

	private void UpdateWinRecordLabel(int playerXWins, int playerOWins)
	{
		_winRecordLabel.Text = $"X  |  {playerXWins} x {playerOWins}  |  O";
	}

	private void ReturnToInitialScreen()
	{
		GetTree().ChangeSceneToFile(_initialScreenPath);
	}

	private void ReloadScene()
	{
		GetTree().ReloadCurrentScene();
	}
}
