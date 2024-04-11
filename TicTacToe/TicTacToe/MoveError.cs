using FluentResults;

namespace TicTacToe;

public abstract class MoveError : Error
{
    protected MoveError(string message) : base(message)
    {
    }
}

public sealed class WrongPlayerTurnError : MoveError
{
    public WrongPlayerTurnError(Player player) 
        : base($"It is not Player {player}'s  turn")
    {
    }
}

public sealed class InvalidPlayerError : MoveError
{
    public InvalidPlayerError(Player player)
        : base($"Unexpected Player '{player}'")
    {
    }
}

public sealed class LocationAlreadyTakenError : MoveError
{
    public LocationAlreadyTakenError(Location location) 
        : base($"The Location {location} is already taken")
    {
    }
}

public sealed class MatchAlreadyFinishedError : MoveError
{
    public MatchAlreadyFinishedError() 
        : base("Game is already finished")
    {
    }
}