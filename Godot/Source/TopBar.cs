using Godot;
using TicTacGodot.CannonMode;
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

	private static int _playerOWins;
	private static int _playerXWins;

	public override void _Ready()
	{
		_matchService = GetNode<MatchService>(AutoloadPath.MatchService);
		_matchService.StartMatch();

		_matchService.MatchStateChanged += OnMatchStateChanged;
		_restartButton.Pressed += OnRestartButtonPressed;
		_exitButton.Pressed += OnExitButtonPressed;

		UpdatePlayerLabel(_matchService.CurrentState);
		UpdateWinRecordLabel();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_matchService.MatchStateChanged -= OnMatchStateChanged;
		}

		base.Dispose(disposing);
	}

	private void OnMatchStateChanged(MatchState state) => UpdateUi(state);
	private void OnExitButtonPressed() => ReturnToInitialScreen();
	private void OnRestartButtonPressed() => ReloadScene();

	private void UpdateUi(MatchState state)
	{
		UpdatePlayerLabel(state);

		if (state is MatchOverState matchOverState)
		{
			RegisterWin(matchOverState);
			UpdateWinRecordLabel();
		}
	}

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

	private void RegisterWin(MatchOverState matchOverState)
	{
		if (matchOverState is not PlayerWonState playerWonState)
			return;

		switch (playerWonState.Winner)
		{
			case Player.X:
				_playerXWins += 1;
				break;

			case Player.O:
				_playerOWins += 1;
				break;
		}
	}

	private void UpdateWinRecordLabel()
	{
		_winRecordLabel.Text = $"X  |  {_playerXWins} x {_playerOWins}  |  O";
	}

	private void ReturnToInitialScreen()
	{
		ChallengeRandomizer.ResetChallengeProgress();
		GetTree().ChangeSceneToFile(_initialScreenPath);
	}

	private void ReloadScene()
	{
		GetTree().ReloadCurrentScene();
	}
}
