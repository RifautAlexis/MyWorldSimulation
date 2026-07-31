using Godot;

namespace Colony.Godot.Scripts.Services;

public class CameraController
{
    public Camera3D CreateCamera()
    {
        var camera = new Camera3D
        {
            Position = new Vector3(10, 10, 10),
        };
        
        camera.LookAtFromPosition(camera.Position, new Vector3(0, 0, 0));

        return camera;
    }
}