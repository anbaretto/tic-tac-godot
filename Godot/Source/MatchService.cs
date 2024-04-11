using System;
using FluentResults;
using Godot;
using TicTacToe;

namespace TicTacToe_Godot;

public partial class MatchService : Node
{
	private Match _match;

	public MatchState CurrentState => _match.State;

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

public interface IPlayerBuildStep
{
	ILocationBuildStep ForPlayer(Player player);
	ILocationBuildStep ForExpectedNextPlayer();
}

public interface ILocationBuildStep
{
	IExecuteBuildStep AtLocation(Location location);
}

public interface IExecuteBuildStep
{
	Result<MatchState> Execute();
}

public sealed class MoveBuilder : IPlayerBuildStep, ILocationBuildStep, IExecuteBuildStep
{
	private Player _player;
	private Location _location;

	private readonly Match _match;
	private readonly Action<MatchState> _matchStateChangedAction;

	public MoveBuilder(Match match, Action<MatchState> matchStateChangedAction)
	{
		_match = match;
		_matchStateChangedAction = matchStateChangedAction;
	}

	ILocationBuildStep IPlayerBuildStep.ForPlayer(Player player)
	{
		_player = player;

		return this;
	}

	ILocationBuildStep IPlayerBuildStep.ForExpectedNextPlayer()
	{
		var nextPlayer = (_match.State as InProgressState)?.NextPlayer ?? Player.None;
		_player = nextPlayer;

		return this;
	}

	IExecuteBuildStep ILocationBuildStep.AtLocation(Location location)
	{
		_location = location;

		return this;
	}

	Result<MatchState> IExecuteBuildStep.Execute()
	{
		var result = _match.MakeAMove(_player, _location);
		GD.Print($"{result}");
		
		if(result.IsSuccess)
			_matchStateChangedAction.Invoke(result.Value);

		return result;
	}
}