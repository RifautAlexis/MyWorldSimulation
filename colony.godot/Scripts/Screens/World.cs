using System;
using Colony.Engine.Simulation;
using Colony.Godot.Scripts.Infrastructure.DependencyInjection;
using Colony.Godot.Scripts.Rendering;
using Colony.Godot.Scripts.Services;
using Colony.Godot.Scripts.UI;
using Godot;

namespace Colony.Godot.Scripts.Screens;

public partial class World : Node3D, IInject<SimulationEngine>, IInject<WorldRenderer>, IInject<CameraController>
{
    private SimulationEngine _simulationEngine = null!;
    private WorldRenderer _worldRenderer = null!;
    private CameraController _cameraController = null!;
    private LayerSelector _layerSelector = null!;
    private CanvasLayer _uiLayer = null!;

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

        SetupCamera();
        CreateLight();
        CreateUI();
    }

    public override void _Process(double delta)
    {
        _cameraController.UpdateMovement(delta);
        _cameraController.UpdateZoom(delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventKey keyEvent)
            return;

        if (!keyEvent.Pressed || keyEvent.Echo)
            return;

        switch (keyEvent.Keycode)
        {
            case Key.A:
                _cameraController.RotateCounterClockwise();
                break;
            case Key.E:
                _cameraController.RotateClockwise();
                break;
            default:
                break;
        }
    }

    private void SetupCamera()
    {
        var initialCenter = new Vector3(_simulationEngine.World.Width / 2, 0, _simulationEngine.World.Height / 2);

        var cameraPivot = new Camera3D
        {
            Name = "CameraPivot",
        };

        AddChild(cameraPivot);

        var camera = new Camera3D
        {
            Name = "Camera3D"
        };

        cameraPivot.AddChild(camera);

        camera.Current = true;

        _cameraController.Initialize(cameraPivot, camera, initialCenter);
    }

    private void CreateLight()
    {
        var light = new DirectionalLight3D();

        light.RotationDegrees = new Vector3(-45, 45, 0);

        AddChild(light);
    }

    private void OnLayerSelected(int layer)
    {
        _worldRenderer.SetSelectedLayer(layer);
    }

    private void CreateUI()
    {
        _uiLayer = new CanvasLayer
        {
            Name = "UI"
        };

        AddChild(_uiLayer);

        CreateLayerSelector();
    }

    private void CreateLayerSelector()
    {
        _layerSelector = new LayerSelector();

        _layerSelector.LayerSelected += OnLayerSelected;

        _uiLayer.AddChild(_layerSelector);

        _layerSelector.Initialize(0, _simulationEngine.World.LayerCount - 1, 0);

        _layerSelector.SetAnchorsPreset(Control.LayoutPreset.TopRight);
        _layerSelector.OffsetLeft = -120;
        _layerSelector.OffsetTop = 30;
        _layerSelector.OffsetRight = -20;
        _layerSelector.OffsetBottom = 280;
    }
}