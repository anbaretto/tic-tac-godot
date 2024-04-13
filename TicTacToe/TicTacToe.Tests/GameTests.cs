#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace TicTacToe.Tests;

public class GameTests
{
    private Match _defaultMatch;
    private Player _playerX;
    private Player _playerO;

    [SetUp]
    public void Setup()
    {
        _defaultMatch = new Match();
        _playerX = Player.X;
        _playerO = Player.O;
    }

    [Test]
    public void Game_WhenPlayerMakesAMove_PlayerIsLast()
    {
        _defaultMatch.MakeAMove(_playerX, Location.BottomCenter);
        Assert.That(((InProgressState)_defaultMatch.State).LastPlayer, Is.EqualTo(_playerX));

        _defaultMatch.MakeAMove(_playerO, Location.TopCenter);
        Assert.That(((InProgressState)_defaultMatch.State).LastPlayer, Is.EqualTo(_playerO));
    }

    [Test]
    public void Game_WhenStarted_IsInProgressState()
    {
        Assert.That(_defaultMatch.State, Is.TypeOf<InProgressState>());
    }

    #region Win Conditions

    [Test]
    public void Game_WhenPlayerFillsHorizontalTop_PlayerWon()
    {
        var game = new Match(Board.Custom()
            .WithMove(_playerX, Location.TopLeft)
            .WithMove(_playerX, Location.TopCenter)
            .WithMove(_playerX, Location.TopRight)
            .Build());

        AssertPlayerWon(game, _playerX);
    }

    [Test]
    public void Game_WhenPlayerFillsHorizontalMiddle_PlayerWon()
    {
        var game = new Match(Board.Custom()
            .WithMove(_playerX, Location.MiddleLeft)
            .WithMove(_playerX, Location.MiddleCenter)
            .WithMove(_playerX, Location.MiddleRight)
            .Build());

        AssertPlayerWon(game, _playerX);
    }

    [Test]
    public void Game_WhenPlayerFillsHorizontalBottom_PlayerWon()
    {
        var game = new Match(Board.Custom()
            .WithMove(_playerX, Location.BottomLeft)
            .WithMove(_playerX, Location.BottomCenter)
            .WithMove(_playerX, Location.BottomRight)
            .Build());

        AssertPlayerWon(game, _playerX);
    }

    [Test]
    public void Game_WhenPlayerFillsVerticalLeft_PlayerWon()
    {
        var game = new Match(Board.Custom()
            .WithMove(_playerX, Location.TopLeft)
            .WithMove(_playerX, Location.MiddleLeft)
            .WithMove(_playerX, Location.BottomLeft)
            .Build());

        AssertPlayerWon(game, _playerX);
    }

    [Test]
    public void Game_WhenPlayerFillsVerticalCenter_PlayerWon()
    {
        var game = new Match(Board.Custom()
            .WithMove(_playerX, Location.TopCenter)
            .WithMove(_playerX, Location.MiddleCenter)
            .WithMove(_playerX, Location.BottomCenter)
            .Build());

        AssertPlayerWon(game, _playerX);
    }

    [Test]
    public void Game_WhenPlayerFillsVerticalRight_PlayerWon()
    {
        var game = new Match(Board.Custom()
            .WithMove(_playerX, Location.TopRight)
            .WithMove(_playerX, Location.MiddleRight)
            .WithMove(_playerX, Location.BottomRight)
            .Build());

        AssertPlayerWon(game, _playerX);
    }

    [Test]
    public void Game_WhenPlayerFillsDiagonal1_PlayerWon()
    {
        var game = new Match(Board.Custom()
            .WithMove(_playerX, Location.TopLeft)
            .WithMove(_playerX, Location.MiddleCenter)
            .WithMove(_playerX, Location.BottomRight)
            .Build());

        AssertPlayerWon(game, _playerX);
    }

    [Test]
    public void Game_WhenPlayerFillsDiagonal2_PlayerWon()
    {
        var game = new Match(Board.Custom()
            .WithMove(_playerX, Location.TopRight)
            .WithMove(_playerX, Location.MiddleCenter)
            .WithMove(_playerX, Location.BottomLeft)
            .Build());

        AssertPlayerWon(game, _playerX);
    }

    private static void AssertPlayerWon(Match match, Player player)
    {
        Assert.That(match.State, Is.TypeOf<PlayerWonState>());
        Assert.That((match.State as PlayerWonState)!.Winner, Is.EqualTo(player));
    }

    #endregion

    #region Board Full -> Tie

    [Test]
    public void Game_WhenPlayerFillsSequenceAtLastMove_PlayerWonState()
    {
        var match = new Match(Board.Custom()
            .WithMove(_playerX, Location.TopLeft)
            .WithMove(_playerO, Location.TopCenter)
            .WithMove(_playerO, Location.MiddleLeft)
            .WithMove(_playerX, Location.MiddleCenter)
            .WithMove(_playerX, Location.MiddleRight)
            .WithMove(_playerX, Location.BottomLeft)
            .WithMove(_playerO, Location.BottomCenter)
            .WithMove(_playerO, Location.BottomRight)
            .Build());

        match.MakeAMove(_playerX, Location.TopRight);

        AssertPlayerWon(match, _playerX);
    }
    
    [Test]
    public void Game_WhenBoardIsFullWithNoWinningPlayers_TieState()
    {
        var match = new Match(Board.Custom()
            .WithMove(_playerX, Location.TopLeft)
            .WithMove(_playerO, Location.TopCenter)
            .WithMove(_playerO, Location.MiddleLeft)
            .WithMove(_playerO, Location.MiddleCenter)
            .WithMove(_playerX, Location.MiddleRight)
            .WithMove(_playerX, Location.BottomLeft)
            .WithMove(_playerX, Location.BottomCenter)
            .WithMove(_playerO, Location.BottomRight)
            .Build());

        match.MakeAMove(_playerX, Location.TopRight);

        Assert.That(match.State, Is.TypeOf<TieState>());
    }

    #endregion

    #region Invalid Moves -> MoveError Results

    [Test]
    public void Game_SamePlayerTriesToPlayTwice_ReturnsWrongPlayerTurnError()
    {
        _defaultMatch.MakeAMove(_playerX, Location.TopLeft);

        var secondMoveResult = _defaultMatch.MakeAMove(_playerX, Location.TopCenter);

        Assert.That(secondMoveResult.IsFailed, Is.True);
        Assert.That(secondMoveResult.HasError<WrongPlayerTurnError>(), Is.True);
    }

    [Test]
    public void Game_PlayerMakesRepeatedMove_ReturnsLocationAlreadyTakenError()
    {
        _defaultMatch.MakeAMove(_playerX, Location.BottomCenter);

        var result = _defaultMatch.MakeAMove(_playerO, Location.BottomCenter);

        Assert.That(result.IsFailed, Is.True);
        Assert.That(result.HasError<LocationAlreadyTakenError>(), Is.True);
    }

    [Test]
    public void Game_WhenOverAndPlayerMakesMove_ReturnsGameAlreadyOverError()
    {
        var game = new Match(Board
            .Custom()
            .WithMove(_playerX, Location.TopLeft)
            .WithMove(_playerX, Location.TopCenter)
            .WithMove(_playerX, Location.TopRight)
            .Build());

        var result = game.MakeAMove(_playerO, Location.BottomRight);

        Assert.That(result.IsFailed, Is.True);
        Assert.That(result.HasError<MatchAlreadyFinishedError>(), Is.True);
    }

    #endregion
}