using Godot;

namespace TicTacGodot.CannonMode;

public partial class ChallengeRandomizer : Node
{
	private static int _challengeIndex;

	public override void _Ready()
	{
		var childrenAmount = GetChildren().Count;

		if (_challengeIndex >= childrenAmount)
			_challengeIndex = 0;

		var challenge = GetChild<AnimationPlayer>(_challengeIndex);
		challenge.Play("challenge_loop");
		
		_challengeIndex += 1;
	}

	public static void ResetChallengeProgress()
	{
		_challengeIndex = 0;
	}
}
