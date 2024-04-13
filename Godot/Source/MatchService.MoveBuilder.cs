using System;
using FluentResults;
using Godot;
using TicTacToe;

namespace TicTacGodot;

public partial class MatchService
{
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

    public sealed class MoveBuilder(
        Match match,
        Action<MatchState> matchStateChangedAction) :
        IPlayerBuildStep, ILocationBuildStep, IExecuteBuildStep
    {
        private Player _player;
        private Location _location;

        ILocationBuildStep IPlayerBuildStep.ForPlayer(Player player)
        {
            _player = player;

            return this;
        }

        ILocationBuildStep IPlayerBuildStep.ForExpectedNextPlayer()
        {
            var nextPlayer = (match.State as InProgressState)?.NextPlayer ?? Player.None;
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
            var result = match.MakeAMove(_player, _location);
            GD.Print($"{result}");

            if (result.IsSuccess)
                matchStateChangedAction.Invoke(result.Value);

            return result;
        }
    }
}