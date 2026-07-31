
using Colony.Godot.Scripts.Services;
using Godot;

namespace Colony.Godot.Scripts.Screens;

public partial class World : Node3D
{
    private readonly CameraController _cameraController;
    
    public override void _Ready()
    {
        CreateCamera();
        CreateLight();

        BuildUI();
    }

    private void BuildUI()
    {
        var center = new CenterContainer();

        center.SetAnchorsAndOffsetsPreset(
            Control.LayoutPreset.FullRect
        );
        AddChild(center);


        var layout = new VBoxContainer();
        center.AddChild(layout);


        var playButton = new Button
        {
            Text = "Popo"
        };
        var exitButton = new Button
        {
            Text = "Toto"
        };

        layout.AddChild(playButton);
        layout.AddChild(exitButton);
    }

    private void CreateCamera()
    {
        var _camera = new Camera3D
        {
            Position = new Vector3(10, 10, 10),
        };
        
        AddChild(_camera);
        
        _camera.LookAt(new Vector3(0, 0, 0));
    }

    private void CreateLight()
    {
        var light = new DirectionalLight3D();

        light.RotationDegrees = new Vector3(-45, 45, 0);

        AddChild(light);
    }
}