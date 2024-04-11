namespace TicTacToe;

public abstract record MatchState;

public record InProgressState(Player NextPlayer) : MatchState
{
    public Player LastPlayer => NextPlayer switch
    {
        Player.None => Player.X,
        Player.O => Player.X,
        Player.X => Player.O,
        _ => throw new ArgumentOutOfRangeException(nameof(NextPlayer))
    };

    public Player NextPlayer { get; internal set; } = NextPlayer;
}

public abstract record MatchOverState : MatchState;
public sealed record TieState : MatchOverState;
public sealed record PlayerWonState(Player Winner) : MatchOverState;