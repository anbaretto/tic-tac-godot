using System;
using Godot;
using TicTacGodot.Utility;
using TicTacToe;

namespace TicTacGodot.CannonMode;

public partial class BoardLocation : Area3D
{
	[Export] private Location Location { get; set; }
	[Export] private Node3D _playerXMark;
	[Export] private Node3D _playerOMark;

	private MatchService _matchService;

	public override void _Ready()
	{
		_matchService = GetNode<MatchService>(AutoloadPath.MatchService);

		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body is not CannonBall cannonBall)
			return;

		var playerHasValue = cannonBall.Player.HasValue;
		if (!playerHasValue)
			throw new InvalidOperationException("Cannon ball without Player assigned!");

		cannonBall.Explode();

		var player = cannonBall.Player.Value;

		var result = _matchService
			.DoMove()
			.ForPlayer(player)
			.AtLocation(Location)
			.Execute();

		if (result.IsSuccess)
		{
			ShowPlayerMark(player);
		}
	}

	private void ShowPlayerMark(Player player)
	{
		var playerMark = player switch
		{
			Player.X => _playerXMark,
			Player.O => _playerOMark,
			_ => throw new ArgumentOutOfRangeException(nameof(player), player, null)
		};

		playerMark.Visible = true;
	}
}
