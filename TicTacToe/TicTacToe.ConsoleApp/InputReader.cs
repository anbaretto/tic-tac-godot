namespace TicTacToe.ConsoleApp;

public class InputReader
{
    public string ReadCharacter()
    {
        var keyInfo = Console.ReadKey();
        var character = keyInfo.KeyChar.ToString();

        return character;
    }
}