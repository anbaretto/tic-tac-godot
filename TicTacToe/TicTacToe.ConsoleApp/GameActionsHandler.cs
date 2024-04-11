using System.Text;
using FluentResults;

namespace TicTacToe.ConsoleApp;

public sealed class GameActionsHandler
{
    private readonly Game _game;
    private readonly Dictionary<string, Func<Result>> _actions = new();

    public GameActionsHandler(Game game)
    {
        _game = game;
    }

    public string GenerateOptions()
    {
        _actions.Clear();
        var messageBuilder = new StringBuilder();

        var appState = _game.State;

        switch (appState)
        {
            case IdleState:
            {
                AddAction(() => _game.State = new RunningGameState(new Match()));
                messageBuilder.AppendLine("1. Start new game");
                break;
            }

            case RunningGameState { Match.State: MatchOverState }:
            {
                messageBuilder.AppendLine("1. End game");
                AddAction(() => _game.State = new IdleState());
                break;
            }

            case RunningGameState { Match.State: InProgressState state } game:
            {
                var match = game.Match;
                var nextPlayer = state.NextPlayer;
                AddAction(() => match.MakeAMove(nextPlayer, Location.TopLeft));
                AddAction(() => match.MakeAMove(nextPlayer, Location.TopCenter));
                AddAction(() => match.MakeAMove(nextPlayer, Location.TopRight));
                AddAction(() => match.MakeAMove(nextPlayer, Location.MiddleLeft));
                AddAction(() => match.MakeAMove(nextPlayer, Location.MiddleCenter));
                AddAction(() => match.MakeAMove(nextPlayer, Location.MiddleRight));
                AddAction(() => match.MakeAMove(nextPlayer, Location.BottomLeft));
                AddAction(() => match.MakeAMove(nextPlayer, Location.BottomCenter));
                AddAction(() => match.MakeAMove(nextPlayer, Location.BottomRight));

                messageBuilder.AppendLine($"It's your turn, Player {nextPlayer.ToString()}");
                break;
            }
        }

        messageBuilder.Append("> ");
        return messageBuilder.ToString();
    }

    public Result PerformAction(string actionCode)
    {
        if (!_actions.TryGetValue(actionCode, out var action))
            return new InvalidActionError(actionCode);

        var performAction = action.Invoke();
        return performAction;
    }

    private void AddAction(Func<Result> action)
    {
        var actionCode = (_actions.Count + 1).ToString();
        _actions.Add(actionCode, action);
    }

    private void AddAction(Func<Result<MatchState>> action)
    {
        AddAction(() =>
        {
            var result = action.Invoke();
            return result.ToResult();
        });
    }

    private void AddAction(Action action)
    {
        AddAction(() =>
        {
            action.Invoke();
            return Result.Ok();
        });
    }
}

public sealed class InvalidActionError : Error
{
    public InvalidActionError(string actionCode)
        : base($"Invalid action: '{actionCode}'")
    {
    }
}