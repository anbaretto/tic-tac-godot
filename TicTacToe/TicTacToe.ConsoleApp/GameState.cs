namespace TicTacToe.ConsoleApp;

public abstract record GameState;
public record IdleState : GameState;
public record RunningGameState(Match Match) : GameState;