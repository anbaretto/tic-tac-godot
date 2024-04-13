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
		var result = _matchService
			.DoMove()
			.ForExpectedNextPlayer()
			.AtLocation(Location)
			.Execute();

		if (!result.IsSuccess)
			return;

		if(result.Value is not InProgressState state)
			return;

		SetPlayerMark(state.LastPlayer);
	}

	private void SetPlayerMark(Player player)
	{
		Text = player.ToString();
	}
}
