using Godot;
using TicTacGodot.Utility;
using TicTacToe;

namespace TicTacGodot;

public delegate void ScoreChangedEventHandler(int xScore, int oScore);

public partial class ScoreCounter : Node
{
    public int OScore { get; private set; }
    public int XScore { get; private set; }

    public event ScoreChangedEventHandler ScoreChanged = delegate { };
    
    private MatchService _matchService;

    public void ResetScores()
    {
        XScore = 0;
        OScore = 0;
    }
    
    public override void _Ready()
    {
        _matchService = GetNode<MatchService>(AutoloadPath.MatchService);
        _matchService.MatchStateChanged += OnMatchStateChanged;
    }
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _matchService.MatchStateChanged -= OnMatchStateChanged;
        }

        base.Dispose(disposing);
    }

    private void OnMatchStateChanged(MatchState state)
    {
        if (state is MatchOverState matchOverState)
        {
            RegisterWin(matchOverState);
        }
    }
    
    private void RegisterWin(MatchOverState matchOverState)
    {
        if (matchOverState is not PlayerWonState playerWonState)
            return;

        switch (playerWonState.Winner)
        {
            case Player.X:
                XScore += 1;
                break;

            case Player.O:
                OScore += 1;
                break;
        }
        
        ScoreChanged.Invoke(XScore, OScore);
    }
}