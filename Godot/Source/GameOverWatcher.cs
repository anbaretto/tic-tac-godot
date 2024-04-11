using Godot;
using TicTacToe;
using TicTacToe_Godot.Utility;

namespace TicTacToe_Godot;

public partial class GameOverWatcher : Node
{
	[Export] private PackedScene _gameOverScreen;

	private MatchService _matchService;

	public override void _Ready()
	{
		_matchService = GetNode<MatchService>(AutoloadPath.MatchService);

		_matchService.StartMatch();
		_matchService.MatchStateChanged += OnMatchStateChanged;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_matchService.MatchStateChanged -= OnMatchStateChanged;
		}

		base.Dispose(disposing);
	}

	private void OnMatchStateChanged(MatchState state)
	{
		if (state is MatchOverState)
		{
			ShowGameOverScreen();
		}
	}

	private void ShowGameOverScreen()
	{
		var node = _gameOverScreen.Instantiate();
		AddChild(node);
	}
}
