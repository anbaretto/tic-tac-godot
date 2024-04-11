namespace TicTacToe;

public class Board
{
    private readonly Player[] _spaces;

    private Board(Player[] spaces)
    {
        _spaces = spaces;
    }

    public static Board Default()
    {
        return new Board(new Player[9]);
    }

    public static BoardBuilder Custom()
    {
        return new BoardBuilder();
    }

    public void Set(Player player, Location location)
    {
        _spaces[(int)location] = player;
    }

    public Player At(Location location)
    {
        return _spaces[(int)location];
    }

    public bool IsFull()
    {
        return _spaces.All(s => s != Player.None);
    }

    public bool IsEmpty()
    {
        return _spaces.All(s => s == Player.None);
    }
}