
using Colony.Engine.Simulation;
using Colony.Godot.Scripts.Infrastructure.DependencyInjection;
using Colony.Godot.Scripts.Rendering;
using Colony.Godot.Scripts.Services;
using Godot;

namespace Colony.Godot.Scripts.Screens;

public partial class World : Node3D, IInject<SimulationEngine>, IInject<WorldRenderer>, IInject<CameraController>
{
    private SimulationEngine _simulationEngine = null!;
    private WorldRenderer _worldRenderer = null!;
    private CameraController _cameraController = null!;

    public void Inject(SimulationEngine simulationEngine)
    {
        _simulationEngine = simulationEngine;
    }
    public void Inject(WorldRenderer worldRenderer)
    {
        _worldRenderer = worldRenderer;
    }
    public void Inject(CameraController cameraController)
    {
        _cameraController = cameraController;
    }

    public override void _Ready()
    {
        var world = _worldRenderer.Build(_simulationEngine.World);
        
        AddChild(world);

        // worldRenderer.ShowOnlyLayer(0);
        _worldRenderer.SetLayerVisible(0, true);
        _worldRenderer.SetLayerVisible(1, false);
        _worldRenderer.SetLayerVisible(2, true);
        
        SetupCamera();
        CreateLight();
    }

    public override void _Process(double delta)
    {
        _cameraController.UpdateMovement(delta);
    }

    private void SetupCamera()
    {
        var camera = new Camera3D
        {
            Position = new Vector3(10, 10, 10),
        };
        
        AddChild(camera);
        
        camera.LookAt(new Vector3(0, 0, 0));
        camera.Current = true;
        
        _cameraController.Initialize(camera);
    }

    private void CreateLight()
    {
        var light = new DirectionalLight3D();

        light.RotationDegrees = new Vector3(-45, 45, 0);

        AddChild(light);
    }
}