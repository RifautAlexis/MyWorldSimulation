using Godot;

namespace Colony.Godot.Scripts.Services;

public class CameraController
{
    private Node3D _cameraPivot = null!;
    private Camera3D _camera = null!;

    private Vector3 _focusPoint = Vector3.Zero;

    private float _zoomDistance = 20.0f;
    private const float MinZoomDistance = 5.0f;
    private const float MaxZoomDistance = 40.0f;
    private const float ZoomSpeed = 10.0f;

    private const float MovementSpeed = 10.0f;

    private int _rotationQuarterTurns = 0;

    public void Initialize(Node3D cameraPivot, Camera3D camera, Vector3 focusPoint)
    {
        _cameraPivot = cameraPivot;
        _camera = camera;
        _focusPoint = focusPoint;

        ApplyCameraTransform();
    }

    public void SetFocusPoint(Vector3 focusPoint)
    {
        _focusPoint = focusPoint;
        
        ApplyCameraTransform();
    }

    public void UpdateMovement(double delta)
    {
        if (_camera == null)
            return;

        var forwardInput = 0.0f;
        var rightInput = 0.0f;

        if (Input.IsKeyPressed(Key.Up))
            forwardInput += 1.0f;
        if (Input.IsKeyPressed(Key.Down))
            forwardInput -= 1.0f;
        if (Input.IsKeyPressed(Key.Right))
            rightInput += 1.0f;
        if (Input.IsKeyPressed(Key.Left))
            rightInput -= 1.0f;

        if (Mathf.IsZeroApprox(forwardInput) && Mathf.IsZeroApprox(rightInput))
            return;
        
        // Direction from camera toward the focus point.
        var forward = _focusPoint - _camera.GlobalPosition;

        // We only want horizontal movement.
        forward.Y = 0;

        forward = forward.Normalized();

        // Get the direction to the camera's right.
        var right = forward.Cross(Vector3.Up).Normalized();

        var direction = forward * forwardInput + right * rightInput;
        
        direction = direction.Normalized();

        // Apply speed and delta
        direction *= MovementSpeed * (float)delta;

        _focusPoint += direction;

        ApplyCameraTransform();
    }

    public void UpdateZoom(double delta)
    {
        if (_camera == null)
            return;

        var zoomDirection = 0.0f;

        if (Input.IsKeyPressed(Key.Pageup))
            zoomDirection -= 1.0f;
        if (Input.IsKeyPressed(Key.Pagedown))
            zoomDirection += 1.0f;

        if (Mathf.IsZeroApprox(zoomDirection))
            return;

        _zoomDistance += zoomDirection * ZoomSpeed * (float)delta;

        _zoomDistance = Mathf.Clamp(
            _zoomDistance,
            MinZoomDistance,
            MaxZoomDistance
        );

        ApplyCameraTransform();
    }

    public void RotateClockwise()
    {
        _rotationQuarterTurns++;

        if (_rotationQuarterTurns >= 4)
        {
            _rotationQuarterTurns = 0;
        }

        ApplyCameraTransform();
    }

    public void RotateCounterClockwise()
    {
        _rotationQuarterTurns--;

        if (_rotationQuarterTurns < 0)
        {
            _rotationQuarterTurns = 3;
        }

        ApplyCameraTransform();
    }

    private void ApplyCameraTransform()
    {
        if (_cameraPivot == null || _camera == null)
            return;

        // The pivot is always located at the point
        // around which the camera orbits.
        _cameraPivot.Position = _focusPoint;

        // Rotate the pivot around the world's Y axis.
        _cameraPivot.Rotation = new Vector3(
            0,
            Mathf.DegToRad(_rotationQuarterTurns * 90.0f),
            0);

        // Camera position is LOCAL to the pivot.
        var cameraDirection = new Vector3(1, 1, 1).Normalized();

        _camera.Position = cameraDirection * _zoomDistance;

        // Look toward the pivot's origin.
        _camera.LookAt(_cameraPivot.GlobalPosition);
    }
}