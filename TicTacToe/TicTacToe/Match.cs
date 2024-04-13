
using FluentResults;

namespace TicTacToe;

public enum Player
{
    None,
    X,
    O,
}

public static class PlayerExtension
{
    public static Player Other(this Player player)
    {
        return player switch
        {
            Player.None => Player.X,
            Player.O => Player.X,
            Player.X => Player.O,
            _ => throw new ArgumentOutOfRangeException(nameof(player))
        };
    }
}

public enum Location
{
    TopLeft = 0,
    TopCenter = 1,
    TopRight = 2,
    MiddleLeft = 3,
    MiddleCenter = 4,
    MiddleRight = 5,
    BottomLeft = 6,
    BottomCenter = 7,
    BottomRight = 8,
}

public sealed class Match
{
    private readonly Board _board;

    public Match(Board board)
    {
        _board = board;

        CheckGameOverConditions();
    }

    public Match()
    {
        _board = Board.Default();
    }

    public MatchState State { get; private set; } = new InProgressState(Player.X);

    public Result<MatchState> MakeAMove(Player player, Location location)
    {
        if (State is not InProgressState state)
            return new MatchAlreadyFinishedError();

        if (player == Player.None)
            return new InvalidPlayerError(player);

        if (player != state.NextPlayer)
            return new WrongPlayerTurnError(player);

        if (_board.At(location) != Player.None)
            return new LocationAlreadyTakenError(location);

        _board.Set(player, location);

        state.NextPlayer = player.Other();

        CheckGameOverConditions();

        return State;
    }

    public Player ValueAt(Location location)
    {
        return _board.At(location);
    }

    public bool JustBegun()
    {
        return _board.IsEmpty();
    }

    private void CheckGameOverConditions()
    {
        if (AnyWinConditionSequenceMatchesFor(out var player))
        {
            State = new PlayerWonState(player);
        }
        else if (_board.IsFull())
        {
            State = new TieState();
        }
    }

    private bool AnyWinConditionSequenceMatchesFor(out Player player)
    {
        if (MakesASequence(
                Location.TopLeft,
                Location.TopCenter,
                Location.TopRight,
                out player))
        {
            return true;
        }

        if (MakesASequence(
                Location.MiddleLeft,
                Location.MiddleCenter,
                Location.MiddleRight,
                out player))
        {
            return true;
        }

        if (MakesASequence(
                Location.BottomLeft,
                Location.BottomCenter,
                Location.BottomRight,
                out player))
        {
            return true;
        }

        if (MakesASequence(
                Location.TopLeft,
                Location.TopCenter,
                Location.TopRight,
                out player))
        {
            return true;
        }

        if (MakesASequence(
                Location.TopLeft,
                Location.MiddleLeft,
                Location.BottomLeft,
                out player))
        {
            return true;
        }

        if (MakesASequence(
                Location.TopCenter,
                Location.MiddleCenter,
                Location.BottomCenter,
                out player))
        {
            return true;
        }

        if (MakesASequence(
                Location.TopRight,
                Location.MiddleRight,
                Location.BottomRight,
                out player))
        {
            return true;
        }
        
        if (MakesASequence(
                Location.TopLeft,
                Location.MiddleCenter,
                Location.BottomRight,
                out player))
        {
            return true;
        }
        
        if (MakesASequence(
                Location.TopRight,
                Location.MiddleCenter,
                Location.BottomLeft,
                out player))
        {
            return true;
        }

        return false;
    }

    private bool MakesASequence(
        Location location1,
        Location location2,
        Location location3,
        out Player player)
    {
        var playerAtLocation1 = _board.At(location1);
        var playerAtLocation2 = _board.At(location2);
        var playerAtLocation3 = _board.At(location3);

        if (playerAtLocation1 == Player.None ||
            playerAtLocation2 == Player.None ||
            playerAtLocation3 == Player.None)
        {
            player = Player.None;
            return false;
        }

        if (_board.At(location1) != _board.At(location2) ||
            _board.At(location2) != _board.At(location3))
        {
            player = Player.None;
            return false;
        }

        player = _board.At(location1);
        return true;
    }
}