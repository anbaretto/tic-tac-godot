using Godot;
using TicTacToe;
using TicTacToe_Godot.Utility;

namespace TicTacToe_Godot.CannonMode;

public partial class CannonBall : RigidBody3D
{
	[Export] private PackedScene _explodeParticlesScene;
	[Export] private AudioStreamPlayer2D _hitAudioPlayer;

	private CameraShaker _cameraShaker;

	public Player? Player { get; set; }

	public override void _Ready()
	{
		_cameraShaker = GetNode<CameraShaker>(AutoloadPath.CameraShaker);

		BodyEntered += OnBodyEntered;
	}

	public void Explode()
	{
		ShakeScreen();
		SpawnParticles();

		Free();
	}

	private void OnBodyEntered(Node body)
	{
		PlayHitAudio();
		ShakeScreen();
	}

	private void PlayHitAudio()
	{
		_hitAudioPlayer.PitchScale = 1 + new RandomNumberGenerator().RandfRange(-0.2f, 0.2f);
		_hitAudioPlayer.Play();
	}

	private void ShakeScreen()
	{
		_cameraShaker.Shake(0.02f, 0.02f);
	}

	private void SpawnParticles()
	{
		var particles = (CpuParticles3D)_explodeParticlesScene.Instantiate();

		GetTree().Root.AddChild(particles);
		particles.GlobalPosition = GlobalPosition;
		particles.Emitting = true;
	}
}
