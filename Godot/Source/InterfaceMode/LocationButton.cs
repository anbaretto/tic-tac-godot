using Godot;
using TicTacGodot.Utility;
using TicTacToe;

namespace TicTacGodot.InterfaceMode;

public partial class LocationButton : Button
{
	[Export] private Location Location { get; set; }

	private MatchService _matchService;

	public override void _Ready()
	{
		_matchService = GetNode<MatchService>(AutoloadPath.MatchService);

		Pressed += OnButtonPressed;
	}

	private void OnButtonPressed()
	{
		var nextPlayer = _matchService.NextPlayer;

		var result = _matchService
			.DoMove()
			.ForPlayer(nextPlayer)
			.AtLocation(Location)
			.Execute();

		if (result.IsSuccess)
		{
			ShowPlayerMark(nextPlayer);
		}
	}

	private void ShowPlayerMark(Player player)
	{
		Text = player.ToString();
	}
}
