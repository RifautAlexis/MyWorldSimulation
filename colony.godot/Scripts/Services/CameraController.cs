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
    private const float MousePanSensitivity = 0.0025f;

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

            if (_isMovingWithMouse)
                if (MoveFocus(emm.Relative))
                    ApplyCameraTransform();
        }
    }

    public void Update(double delta)
    {
        if (_camera == null)
            return;

        var hasChanged = false;

        hasChanged |= UpdateKeyboardMovement(delta);
        hasChanged |= UpdateKeyboardZoom(delta);
        hasChanged |= UpdateRotation(delta);

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

    private bool MoveFocus(Vector2 screenDelta)
    {
        // Get the camera's current forward direction.
        // We don't use _focusPoint - _camera.GlobalPosition here because
        // we want the camera orientation itself to define the movement.
        var cameraForward = -_camera.GlobalTransform.Basis.Z;

        // Project the forward direction onto the horizontal XZ plane.
        // Not moving camera vertically and its inclination
        cameraForward.Y = 0;

        if (cameraForward.LengthSquared() < Mathf.Epsilon)
            return false;

        cameraForward = cameraForward.Normalized();

        // Get the camera's right direction.
        var cameraRight = _camera.GlobalTransform.Basis.X;

        // Project the right direction onto the horizontal XZ plane.
        // Not moving camera vertically and its inclination
        cameraRight.Y = 0;

        if (cameraRight.LengthSquared() < Mathf.Epsilon)
            return false;

        cameraRight = cameraRight.Normalized();

        var movement = -cameraRight * screenDelta.X + cameraForward * screenDelta.Y;

        // Make panning speed depend on the current zoom distance.
        // Zoomed in  -> slower, more precise movement
        // Zoomed out -> faster, larger movement
        var panSpeed = MousePanSensitivity * _zoomDistance;

        movement *= panSpeed;

        _focusPoint += movement;

        return true;
    }

    private bool UpdateKeyboardMovement(double delta)
    {
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

        if (Mathf.IsZeroApprox(forwardInput) &&
            Mathf.IsZeroApprox(rightInput))
            return false;

        var forward = _focusPoint - _camera.GlobalPosition;

        forward.Y = 0;
        forward = forward.Normalized();

        var right = forward.Cross(Vector3.Up).Normalized();

        var direction = forward * forwardInput + right * rightInput;

        // When player presses both directions
        if (direction.LengthSquared() > 1.0f)
            direction = direction.Normalized();

        _focusPoint += direction * MovementSpeed * (float)delta;

        return true;
    }

    private bool UpdateKeyboardZoom(double delta)
    {
        var zoomDistance = MouseWheelZoomStep * ZoomSpeed * (float)delta;

        var hasChanged = false;

        if (Input.IsKeyPressed(Key.Pageup))
            hasChanged = AdjustZoom(-zoomDistance);

        if (Input.IsKeyPressed(Key.Pagedown))
            hasChanged = AdjustZoom(zoomDistance) || hasChanged;

        return hasChanged;
    }

    private bool UpdateRotation(double delta)
    {
        var smoothingFactor =
            1.0f - Mathf.Exp(-RotationSmoothingSpeed * (float)delta);

        var newYaw = Mathf.LerpAngle(
            _yaw,
            _targetYaw,
            smoothingFactor);

        var newPitch = Mathf.Lerp(
            _pitch,
            _targetPitch,
            smoothingFactor);

        if (Mathf.IsEqualApprox(_yaw, newYaw) &&
            Mathf.IsEqualApprox(_pitch, newPitch))
            return false;

        _yaw = newYaw;
        _pitch = newPitch;

        return true;
    }
}