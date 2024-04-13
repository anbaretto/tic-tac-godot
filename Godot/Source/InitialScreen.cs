using Godot;
using TicTacGodot.CannonMode;
using TicTacGodot.Utility;

namespace TicTacGodot;

public partial class InitialScreen : Panel
{
    [Export] private Button _playInterfaceButton;
    [Export] private Button _playCannonButton;
    [Export] private Button _exitButton;

    [Export(PropertyHint.File, FileType.Scene)]
    private string _interfaceModeScenePath;

    [Export(PropertyHint.File, FileType.Scene)]
    private string _cannonModeScenePath;

    public override void _Ready()
    {
        _playInterfaceButton.Pressed += OnPlayInterfaceButtonPressed;
        _playCannonButton.Pressed += OnPlayCannonButtonPressed;
        _exitButton.Pressed += OnExitButtonPressed;

        ResetMetaProgress();
    }

    private void OnPlayInterfaceButtonPressed() => LoadMatch(_interfaceModeScenePath);
    private void OnPlayCannonButtonPressed() => LoadMatch(_cannonModeScenePath);
    private void OnExitButtonPressed() => ExitApplication();


    private void LoadMatch(string scenePath)
    {
        GetTree().ChangeSceneToFile(scenePath);
    }

    private void ExitApplication()
    {
        GetTree().Quit();
    }

    private void ResetMetaProgress()
    {
        GetNode<ScoreCounter>(AutoloadPath.ScoreCounter).ResetScores();
        ChallengeRandomizer.ResetChallengeProgress();
    }
}