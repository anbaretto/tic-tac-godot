using System;
using Godot;
using TicTacToe;

namespace TicTacGodot;

public partial class MatchService : Node
{
	private Match _match;

	public MatchState CurrentState => _match.State;
	public Player NextPlayer => (_match.State as InProgressState)?.NextPlayer ?? Player.None;

	public event Action<MatchState> MatchStateChanged = delegate {  };

	public void StartMatch()
	{
		_match = new Match();
		MatchStateChanged?.Invoke(_match.State);
	}
	
	public IPlayerBuildStep DoMove()
	{
		return new MoveBuilder(_match, MatchStateChanged.Invoke);
	}
}