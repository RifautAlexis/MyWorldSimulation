using Colony.Engine.Simulation;
using Colony.Godot.Scripts.Infrastructure.DependencyInjection;
using Colony.Godot.Scripts.Rendering;
using Colony.Godot.Scripts.Services;
using Colony.Godot.Scripts.UI;
using Godot;

namespace Colony.Godot.Scripts.Screens;

public partial class WorldScreen : Node3D,
                                   IInject<WorldRenderer>,
                                   IInject<CameraController>,
                                   IInject<ColonistRenderer>

{
    private CameraController _cameraController = null!;
    private ColonistRenderer _colonistRenderer = null!;

    private Node3D _colonistRoot = null!;
    private LayerSelector _layerSelector = null!;
    private ColonySimulation _simulation = null!;

    private Label _timeLabel = null!;
    private CanvasLayer _uiLayer = null!;
    private WorldRenderer _worldRenderer = null!;

    public void Inject(CameraController cameraController)
    {
        _cameraController = cameraController;
    }

    public void Inject(ColonistRenderer colonistRenderer)
    {
        _colonistRenderer = colonistRenderer;
    }

    public void Inject(WorldRenderer worldRenderer)
    {
        _worldRenderer = worldRenderer;
    }

    public void Initialize(ColonySimulation simulation)
    {
        _simulation = simulation;
    }

    public override void _Ready()
    {
        var world = _worldRenderer.Build(_simulation.World);
        // Initialize(_simulation);

        AddChild(world);

        // CreateColonists();

        SetupCamera();
        CreateLight();
        CreateUI();
    }

    public override void _Process(double delta)
    {
        _simulation.Tick(delta);

        // UpdateColonists();

        _cameraController.UpdateMovement(delta);
        _cameraController.UpdateZoom(delta);

        UpdateTimeLabel();
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
            case Key.Space:
                TogglePause();
                break;
            case Key.F1:
                _simulation.SetSpeed(0.5);
                break;

            case Key.F2:
                _simulation.SetSpeed(1.0);
                break;

            case Key.F3:
                _simulation.SetSpeed(2.0);
                break;

            case Key.F4:
                _simulation.SetSpeed(5.0);
                break;
        }
    }

    private void SetupCamera()
    {
        var initialCenter = new Vector3(_simulation.World.Width / 2, 0, _simulation.World.Height / 2);

        var cameraPivot = new Camera3D
        {
            Name = "CameraPivot",
        };

        AddChild(cameraPivot);

        var camera = new Camera3D
        {
            Name = "Camera3D",
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
            Name = "UI",
        };

        AddChild(_uiLayer);

        CreateTimeLabel();
        CreateLayerSelector();
    }

    private void CreateLayerSelector()
    {
        _layerSelector = new LayerSelector();

        _layerSelector.LayerSelected += OnLayerSelected;

        _uiLayer.AddChild(_layerSelector);

        _layerSelector.Initialize(0, _simulation.World.LayerCount - 1, 0);

        _layerSelector.SetAnchorsPreset(Control.LayoutPreset.TopRight);
        _layerSelector.OffsetLeft = -120;
        _layerSelector.OffsetTop = 30;
        _layerSelector.OffsetRight = -20;
        _layerSelector.OffsetBottom = 280;
    }

    private void CreateTimeLabel()
    {
        _timeLabel = new Label
        {
            Text = "Day 0 - 00:00 (0 ticks)",
        };

        _timeLabel.Position = new Vector2(20, 20);

        _uiLayer.AddChild(_timeLabel);
    }

    private void UpdateTimeLabel()
    {
        var time = _simulation.GameTime;

        _timeLabel.Text = $"Day {time.Day} - {time.Hour:00}:{time.Minute:00} ({_simulation.TickNumber} ticks)";
    }

    private void TogglePause()
    {
        if (_simulation.IsPaused)
            _simulation.Resume();
        else
            _simulation.Pause();
    }

    private void CreateColonists()
    {
        _colonistRoot = new Node3D
        {
            Name = "Colonists",
        };

        AddChild(_colonistRoot);

        foreach (var colonist in _simulation.Colonists)
        {
            var node = _colonistRenderer.Build(colonist);

            _colonistRoot.AddChild(node);
        }
    }

    private void UpdateColonists()
    {
        foreach (var colonist in _simulation.Colonists) _colonistRenderer.Update(colonist);
    }
}