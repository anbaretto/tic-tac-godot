using FluentResults;

namespace TicTacToe.ConsoleApp;

public sealed class Game
{
    private const string GameQuittingCode = "0";

    private readonly Printer _printer;
    private readonly InputReader _inputReader;
    private readonly GameDrawer _gameDrawer;
    private readonly GameActionsHandler _gameActionsHandler;

    public GameState State { get; set; }

    private Result _lastActionResult = Result.Ok();

    public Game(Printer printer, InputReader inputReader, GameDrawer gameDrawer)
    {
        State = new IdleState();
        _printer = printer;
        _inputReader = inputReader;
        _gameDrawer = gameDrawer;
        _gameActionsHandler = new(this);
    }

    public void ShowMatchState()
    {
        _printer.Clear();

        PrintLastActionResult();

        if (State is IdleState)
        {
            PrintWelcome();
            return;
        }

        if (State is not RunningGameState runningGameState)
            return;

        if (runningGameState.Match.State is MatchOverState matchOverState)
        {
            PrintMatchOverResult(matchOverState);
        }

        var matchView = _gameDrawer.GenerateMatchView(runningGameState.Match);
        _printer.PrintLine(matchView);
    }

    public void GenerateAndShowActionOptions()
    {
        var options = _gameActionsHandler.GenerateOptions();
        _printer.Print(options);
    }

    public string WaitForInput()
    {
        var inputChar = _inputReader.ReadCharacter();

        if (inputChar == GameQuittingCode)
            throw new GameQuitException();

        return inputChar;
    }

    public void PerformAction(string optionCode)
    {
        _lastActionResult = _gameActionsHandler.PerformAction(optionCode);
    }


    private void PrintMatchOverResult(MatchOverState matchOverState)
    {
        if (matchOverState is TieState)
        {
            _printer.PrintLine("It's a tie!");
            _printer.PrintLine();
        }
        else if (matchOverState is PlayerWonState playerWonState)
        {
            _printer.PrintLine($"Congratulations Player {playerWonState.Winner}, you Won!");
            _printer.PrintLine();
        }
    }

    private void PrintLastActionResult()
    {
        if (_lastActionResult.IsFailed)
        {
            var errorMessage = $"[{_lastActionResult.Errors.First().Message}]";
            _printer.PrintLine(errorMessage);
            _printer.PrintLine();
        }
        else
        {
            _printer.PrintLine();
            _printer.PrintLine();
        }
    }

    private void PrintWelcome()
    {
        _printer.PrintLine("Welcome to TicTacToe - Ultimate Console Edition!");
        _printer.PrintLine();
        _printer.PrintLine("At any time, you can press 0 to quit the game.");
        _printer.PrintLine();
    }
}