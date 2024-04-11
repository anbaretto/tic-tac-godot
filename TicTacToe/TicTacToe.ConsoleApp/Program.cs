using TicTacToe.ConsoleApp;

var printer = new Printer();
var inputReader = new InputReader();
var gameDrawer = new GameDrawer();
var game = new Game(printer, inputReader, gameDrawer);

try
{
    while (true)
    {
        game.ShowMatchState();
        game.GenerateAndShowActionOptions();

        var actionCode = game.WaitForInput();

        game.PerformAction(actionCode);
    }
}
catch (GameQuitException)
{
}