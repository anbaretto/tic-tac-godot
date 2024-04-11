using Godot;

namespace TicTacToe_Godot;

public partial class CameraShaker : Node
{
    private Camera3D _camera;
    private float _intensity;
    private float _duration;

    private bool HasCamera => _camera != null && IsInstanceValid(_camera);

    public override void _Process(double delta)
    {
        if(!HasCamera)
            return;
        
        if(_duration <= 0)
            return;

        DoShake(delta);
    }

    public void Shake(float intensity, float duration)
    {
        if (!HasCamera)
        {
            if (!TrySetCamera())
                return;
        }

        _intensity = intensity;
        _duration = duration;
    }

    private void DoShake(double delta)
    {
        _duration -= (float)delta;

        var random = new RandomNumberGenerator();
        var offset = new Vector2(random.Randf(), random.Randf()) * _intensity;

        _camera.HOffset = offset.X;
        _camera.VOffset = offset.Y;
        
        if (_duration <= 0)
        {
            StopShake();
        }
    }

    private void StopShake()
    {
        _duration = 0;

        _camera.HOffset = 0;
        _camera.VOffset = 0;
    }

    private bool TrySetCamera()
    {
        _camera = GetTree().CurrentScene.GetNode<Camera3D>("Camera3D");
        return HasCamera;
    }
}