namespace TicTacToe.ConsoleApp;

public class Printer
{
    public void Print(string text)
    {
        Console.Write(text);
    }

    public void PrintLine(string text)
    {
        Console.WriteLine(text);
    }

    public void PrintLine()
    {
        Console.WriteLine();
    }

    public void Clear()
    {
        Console.Clear();
    }
}