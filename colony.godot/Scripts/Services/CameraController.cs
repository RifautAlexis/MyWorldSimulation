using System;
using Godot;

namespace Colony.Godot.Scripts.Services;

public class CameraController
{
    private const float MinZoomDistance = 5.0f;
    private const float MaxZoomDistance = 100.0f;
    private const float MouseWheelZoomStep = 1.0f;
    private const float ZoomSpeed = 20.0f;
    private const float MovementSpeed = 20.0f;
    private const float MouseOrbitSensitivity = 0.006f;
    private const float RotationSmoothingSpeed = 16.0f;
    private const float MinPitchDegrees = 20.0f;
    private const float MaxPitchDegrees = 80.0f;
    private const float InitialYawDegrees = 45.0f;
    private const float InitialPitchDegrees = 35.26439f;
    private const float MousePanSensitivity = 0.05f;

    private Camera3D _camera = null!;
    private Node3D _cameraPivot = null!;
    private Vector3 _focusPoint = Vector3.Zero;
    private bool _isMovingWithMouse;
    private bool _isOrbitingWithMouse;
    private float _pitch = Mathf.DegToRad(InitialPitchDegrees);
    private float _targetPitch = Mathf.DegToRad(InitialPitchDegrees);
    private float _targetYaw = Mathf.DegToRad(InitialYawDegrees);
    private float _yaw = Mathf.DegToRad(InitialYawDegrees);
    private float _zoomDistance = 20.0f;

    public void Initialize(Node3D cameraPivot, Camera3D camera, Vector3 focusPoint)
    {
        _cameraPivot = cameraPivot;
        _camera = camera;
        _focusPoint = focusPoint;

        ApplyCameraTransform();
    }

    public void HandleInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton emb)
        {
            switch (emb.ButtonIndex)
            {
                case MouseButton.Right:
                    _isOrbitingWithMouse = emb.Pressed;

                    Input.MouseMode = _isOrbitingWithMouse
                        ? Input.MouseModeEnum.Captured
                        : Input.MouseModeEnum.Visible;

                    break;

                case MouseButton.Left:
                    _isMovingWithMouse = emb.Pressed;
                    break;

                case MouseButton.WheelUp when emb.Pressed:
                    if (AdjustZoom(-MouseWheelZoomStep))
                        ApplyCameraTransform();

                    break;

                case MouseButton.WheelDown when emb.Pressed:
                    if (AdjustZoom(MouseWheelZoomStep))
                        ApplyCameraTransform();

                    break;
            }
        }

        if (@event is InputEventMouseMotion emm)
        {
            if (_isOrbitingWithMouse)
            {
                _targetYaw -= emm.Relative.X * MouseOrbitSensitivity;
                _targetPitch = Mathf.Clamp(
                    _targetPitch - emm.Relative.Y * MouseOrbitSensitivity,
                    Mathf.DegToRad(MinPitchDegrees),
                    Mathf.DegToRad(MaxPitchDegrees)
                );
            }

            if (_isMovingWithMouse) MoveFocus(emm.Relative);
        }
    }

    public void Update(double delta)
    {
        if (_camera == null)
            return;

        var hasChanged = false;

        var forwardInput = 0.0f;
        var rightInput = 0.0f;

        if (Input.IsKeyPressed(Key.Z))
            forwardInput += 1.0f;
        if (Input.IsKeyPressed(Key.S))
            forwardInput -= 1.0f;
        if (Input.IsKeyPressed(Key.D))
            rightInput += 1.0f;
        if (Input.IsKeyPressed(Key.Q))
            rightInput -= 1.0f;

        if (!Mathf.IsZeroApprox(forwardInput) || !Mathf.IsZeroApprox(rightInput))
        {
            // Direction from camera toward the focus point.
            var forward = _focusPoint - _camera.GlobalPosition;

            // We only want horizontal movement.
            forward.Y = 0;

            forward = forward.Normalized();

            // Get the direction to the camera's right.
            var right = forward.Cross(Vector3.Up).Normalized();

            var direction = forward * forwardInput + right * rightInput;

            direction = direction.Normalized();

            // Apply speed and delta.
            direction *= MovementSpeed * (float)delta;

            _focusPoint += direction;
            hasChanged = true;
        }

        var zoomDistance = MouseWheelZoomStep * ZoomSpeed * (float)delta;

        if (Input.IsKeyPressed(Key.Pageup))
            hasChanged = AdjustZoom(-zoomDistance) || hasChanged;

        if (Input.IsKeyPressed(Key.Pagedown))
            hasChanged = AdjustZoom(zoomDistance) || hasChanged;

        var smoothingFactor = 1.0f - Mathf.Exp(-RotationSmoothingSpeed * (float)delta);
        var newYaw = Mathf.LerpAngle(_yaw, _targetYaw, smoothingFactor);
        var newPitch = Mathf.Lerp(_pitch, _targetPitch, smoothingFactor);

        if (!Mathf.IsEqualApprox(_yaw, newYaw) || !Mathf.IsEqualApprox(_pitch, newPitch))
        {
            _yaw = newYaw;
            _pitch = newPitch;
            hasChanged = true;
        }

        if (hasChanged)
            ApplyCameraTransform();
    }

    public void RotateClockwise()
    {
        _targetYaw += Mathf.Pi / 2.0f;
    }

    public void RotateCounterClockwise()
    {
        _targetYaw -= Mathf.Pi / 2.0f;
    }

    private void ApplyCameraTransform()
    {
        if (_cameraPivot == null || _camera == null)
            return;

        // The pivot is always located at the point
        // around which the camera orbits.
        _cameraPivot.Position = _focusPoint;
        _cameraPivot.Rotation = Vector3.Zero;

        var cameraDirection = new Vector3(
            Mathf.Sin(_yaw) * Mathf.Cos(_pitch),
            Mathf.Sin(_pitch),
            Mathf.Cos(_yaw) * Mathf.Cos(_pitch)
        );

        _camera.Position = cameraDirection * _zoomDistance;
        _camera.LookAt(_cameraPivot.GlobalPosition);
    }

    private bool AdjustZoom(float zoomDelta)
    {
        var previousDistance = _zoomDistance;
        _zoomDistance = Mathf.Clamp(
            _zoomDistance + zoomDelta,
            MinZoomDistance,
            MaxZoomDistance
        );

        return !Mathf.IsEqualApprox(previousDistance, _zoomDistance);
    }

    private void MoveFocus(Vector2 screenDelta)
    {
        var forward = _focusPoint - _camera.GlobalPosition;

        forward.Y = 0;

        if (forward.LengthSquared() < Mathf.Epsilon)
            return;

        forward = forward.Normalized();

        var right = forward.Cross(Vector3.Up).Normalized();

        var movement =
            -right * screenDelta.X * MousePanSensitivity +
            -forward * screenDelta.Y * MousePanSensitivity;

        _focusPoint += movement;

        ApplyCameraTransform();
    }
}