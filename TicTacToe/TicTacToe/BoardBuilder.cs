namespace TicTacToe;

public sealed class BoardBuilder
{
    private readonly Board _board = Board.Default();
    
    public BoardBuilder WithMove(Player player, Location location)
    {
        _board.Set(player, location);

        return this;
    }
    
    public Board Build()
    {
        return _board;
    }
}