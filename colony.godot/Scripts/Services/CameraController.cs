using Godot;

namespace Colony.Godot.Scripts.Services;

public class CameraController
{
    private Camera3D _camera = null!;
    
    private const float MovementSpeed = 10.0f;

    public void Initialize(Camera3D camera)
    {
        _camera = camera;
    }

    public void SetPosition(Vector3 position)
    {
        if (_camera == null)
            return;

        _camera.Position = position;
    }
    
    public void UpdateMovement(double delta)
    {
        if (_camera == null)
            return;

        var direction = Vector3.Zero;

        if (Input.IsKeyPressed(Key.Up))
            direction.Z -= 1;
        if (Input.IsKeyPressed(Key.Down))
            direction.Z += 1;
        if (Input.IsKeyPressed(Key.Left))
            direction.X -= 1;
        if (Input.IsKeyPressed(Key.Right))
            direction.X += 1;

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized() * MovementSpeed * (float)delta;
            _camera.Position += direction;
        }
    }
}