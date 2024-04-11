using System;
using Godot;
using TicTacToe;
using TicTacToe_Godot.Utility;

namespace TicTacToe_Godot.CannonMode;

public partial class Cannon : Node3D
{
	[Export] private PackedScene _projectileScene;
	[Export] private Marker3D _fireSpawnLocation;
	[Export] private Node3D _barrel;
	[Export] private Player _player;
	[Export] private Vector2 _projectileForceRange;
	[Export] private float _projectileForceChargingSpeed;
	[Export] private float _rotationSpeed;
	[Export] private float _fireShakeIntensity;
	[Export] private float _fireShakeDuration;
	[Export] private AudioStreamPlayer2D _fireAudioPlayer;

	private InputType.IPlayerInputType _playerInputType;
	private bool _isChargingCannon;
	private float _fireForce;
	private Tween _chargingTween;
	private CameraShaker _cameraShaker;

	public override void _Ready()
	{
		var matchService = GetNode<MatchService>(AutoloadPath.MatchService);
		matchService.StartMatch();

		_cameraShaker = GetNode<CameraShaker>(AutoloadPath.CameraShaker);

		_playerInputType = InputType.ForPlayer(_player);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(_playerInputType.CannonFire))
		{
			StartChargingCannon();
		}

		if (Input.IsActionJustReleased(_playerInputType.CannonFire))
		{
			FireCannon();
		}

		if (_isChargingCannon)
		{
			ChargeCannon(delta);
		}

		RotateCannonFromInput();
	}

	private void ChargeCannon(double delta)
	{
		_fireForce += (float)delta * _projectileForceChargingSpeed;
		_fireForce = Mathf.Min(_fireForce, _projectileForceRange.Y);
	}

	private void StartChargingCannon()
	{
		_isChargingCannon = true;
		_fireForce = _projectileForceRange.X;
		
		_chargingTween = CreateTween();
		_chargingTween.TweenProperty(_barrel, "scale", new Vector3(1, 1, 0.7f), 1);
	}

	private void FireCannon()
	{
		_chargingTween.Kill();
		_barrel.Scale = Vector3.One;

		if (_projectileScene.Instantiate() is not CannonBall cannonBall)
			throw new InvalidOperationException();

		GetTree().Root.AddChild(cannonBall);

		cannonBall.Player = _player;
		cannonBall.GlobalPosition = _fireSpawnLocation.GlobalPosition;

		var impulseDirection = _fireSpawnLocation.GlobalPosition - _barrel.GlobalPosition;
		cannonBall.ApplyImpulse(impulseDirection.Normalized() * _fireForce);

		_isChargingCannon = false;

		_cameraShaker.Shake(.04f, .05f);
		_cameraShaker.Shake(_fireShakeIntensity, _fireShakeDuration);

		_fireAudioPlayer.PitchScale = 1 + new RandomNumberGenerator().RandfRange(-0.2f, 0.2f);
		_fireAudioPlayer.Play();
	}

	private void RotateCannonFromInput()
	{
		var rotationLeft = GetRotationForInput(_playerInputType.CannonRotateLeft);
		var rotationRight = GetRotationForInput(_playerInputType.CannonRotateRight);
		var rotationUp = GetRotationForInput(_playerInputType.CannonRotateUp);
		var rotationDown = GetRotationForInput(_playerInputType.CannonRotateDown);

		_barrel.RotateY(-rotationRight);
		_barrel.RotateY(rotationLeft);
		_barrel.RotateX(-rotationUp);
		_barrel.RotateX(rotationDown);
	}

	private float GetRotationForInput(string inputType)
	{
		var inputStrength = Input.GetActionStrength(inputType);
		return Mathf.DegToRad(inputStrength) * _rotationSpeed;
	}
}
