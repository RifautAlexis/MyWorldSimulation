using System;
using System.Threading;
using System.Threading.Tasks;
using Colony.Engine.Facade;
using Colony.Engine.Facade.Contracts;
using Colony.Godot.Scripts.Infrastructure.Navigation;
using Colony.Godot.Scripts.UI.Controllers;
using Colony.Godot.Scripts.UI.Renderers;
using Colony.Godot.Scripts.UI.Screens.MapGenerationSetup.models;
using Colony.Godot.Scripts.UI.Screens.MapGenerationSetup.UI;
using Godot;

namespace Colony.Godot.Scripts.UI.Screens.MapGenerationSetup;

public class MapGenerationSetupForm
{
    public MapSize MapSize { get; set; }

    public int Seed { get; set; }
}

public partial class MapGenerationSetupScreen : BaseScreen
{
    private const int DefaultLayerCount = 100;

    private readonly CameraController _cameraController = null!;
    private readonly MapRenderer _mapRenderer = null!;
    private readonly IScreenNavigator _navigator = null!;

    private Label _currentStepLabel = null!;

    private MapGenerationSetupForm _form = null!;
    private Button _generateButton = null!;
    private CancellationTokenSource? _generationCts;

    private CanvasLayer _hudLayer = null!;
    private Control _hudRoot = null!;
    private bool _isGenerating;
    private CanvasLayer _loadingLayer = null!;
    private Control _loadingRoot = null!;
    private Node3D _mapContainer = null!;
    private Button _playButton = null!;
    private Label _spinnerLabel = null!;
    private Node3D _worldRoot = null!;

    public MapGenerationSetupScreen(IScreenNavigator navigator) : base(navigator)
    {
        _navigator = navigator;

        _worldRoot = new Node3D
        {
            Name = "WorldRoot",
        };

        var layerRenderer = new LayerRenderer();
        _mapRenderer = new MapRenderer(layerRenderer);
        _cameraController = new CameraController();
    }

    public override async void _Ready()
    {
        _form = InitializeForm();

        AddChild(_worldRoot);
        VisualRoot = _worldRoot;

        BuildMap();
        BuildCamera(_form.MapSize.XAxis, _form.MapSize.YAxis);
        BuildHud();
        _ = RegenerateMapAsync();
    }

    public override void _Process(double delta)
    {
        _cameraController.Update((float)delta);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventKey keyEvent)
            return;

        if (!keyEvent.Pressed || keyEvent.Echo)
            return;

        switch (keyEvent.Keycode)
        {
            // case Key.Escape:
            //     if (_pauseMenu.Visible)
            //     {
            //         _pauseMenu.Hide();
            //         TogglePause();
            //     }
            //     else
            //     {
            //         DisplayPauseMenu();
            //     }
            //
            //     break;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        _cameraController.HandleInput(@event);

        if (@event is not InputEventKey keyEvent) return;

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
        }
    }

    private void BuildMap()
    {
        _mapContainer = new Node3D
        {
            Name = "MapContainer",
        };
        _worldRoot.AddChild(_mapContainer);

        CreateLight();
    }

    private void BuildHud()
    {
        _hudLayer = new CanvasLayer
        {
            Name = "HudLayer",
            Layer = 1,
        };
        AddChild(_hudLayer);

        _hudRoot = new Control
        {
            Name = "HudRoot",
        };
        _hudRoot.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        _hudLayer.AddChild(_hudRoot);

        var ui = new MapGenerationSetupUI(_hudRoot);
        ui.Build(CreateBindings());
        _currentStepLabel = ui.CurrentStepLabel;
        _generateButton = ui.GenerateButton;
        _playButton = ui.PlayButton;
        UpdateGenerateButton();

        BuildLoadingOverlay();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (_hudRoot != null) _hudRoot.Visible = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        if (_hudRoot != null) _hudRoot.Visible = false;
    }

    private void BuildCamera(int xAxisSize, int yAxisSize)
    {
        var initialCenter = new Vector3(xAxisSize / 2, 0, yAxisSize / 2);

        var cameraPivot = new Node3D
        {
            Name = "CameraPivot",
        };

        _worldRoot.AddChild(cameraPivot);

        var camera = new Camera3D
        {
            Name = "Camera3D",
        };

        cameraPivot.AddChild(camera);

        camera.Current = true;
        camera.Near = 0.05f;
        camera.Far = 5000f;

        _cameraController.Initialize(cameraPivot, camera, initialCenter);
    }

    private void CreateLight()
    {
        var light = new DirectionalLight3D();

        light.RotationDegrees = new Vector3(-45, 45, 0);

        _worldRoot.AddChild(light);
    }

    private async Task RegenerateMapAsync()
    {
        if (_isGenerating)
            return;

        _isGenerating = true;
        _generationCts?.Cancel();
        _generationCts?.Dispose();
        _generationCts = new CancellationTokenSource();

        SetLoadingOverlayVisible(true);
        _generateButton.Disabled = true;
        _playButton.Disabled = true;

        var progress = new Progress<MapGenerationProgress>(OnMapGenerationProgress);

        try
        {
            IColonyEngine engine = new ColonyEngineFacade();

            var mapSettings = new MapGenerationSettings
            {
                XAxisSize = _form.MapSize.XAxis,
                YAxisSize = _form.MapSize.YAxis,
                Seed = _form.Seed,
                LayerCount = DefaultLayerCount,
            };
            var generatedMap = await engine.GenerateMapAsync(mapSettings, progress, _generationCts.Token);

            foreach (var child in _mapContainer.GetChildren())
                if (child is Node node)
                {
                    _mapContainer.RemoveChild(node);
                    node.QueueFree();
                }

            var mapPreviewNode = _mapRenderer.Build(generatedMap);
            _mapContainer.AddChild(mapPreviewNode);
            _currentStepLabel.Text = "Current Step: Completed";
        }
        catch (OperationCanceledException)
        {
            _currentStepLabel.Text = "Current Step: Canceled";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _currentStepLabel.Text = "Current Step: Failed";
        }
        finally
        {
            SetLoadingOverlayVisible(false);
            _isGenerating = false;
            UpdateGenerateButton();
        }
    }

    private void OnMapGenerationProgress(MapGenerationProgress progress)
    {
        Console.WriteLine($"Map generation progress: {progress.Step}");
        _currentStepLabel.Text = "Current Step: " + progress.Step;
    }

    private MapGenerationSetupForm InitializeForm()
    {
        return new MapGenerationSetupForm
        {
            MapSize = MapSize.Medium,
            Seed = 12345,
        };
    }

    private bool IsFormValid()
    {
        return _form.MapSize is { XAxis: > 0, YAxis: > 0 } && _form.Seed != null;
    }

    private void SelectMapSize(int xAxis, int yAxis)
    {
        _form.MapSize = new MapSize
        {
            XAxis = xAxis,
            YAxis = yAxis,
        };

        UpdateGenerateButton();
    }

    private MapGenerationSetupBindings CreateBindings()
    {
        return new MapGenerationSetupBindings
        {
            InitialMapSize = _form.MapSize,
            InitialSeed = _form.Seed,
            Generate = OnGeneratePressed,
            Play = OnPlayPressed,
            SelectMapSize = mapSize => SelectMapSize(mapSize.XAxis, mapSize.YAxis),
            UpdateSeed = value =>
            {
                if (value == null) return;
                _form.Seed = value;
            },
        };
    }

    private void UpdateGenerateButton()
    {
        _generateButton.Disabled = !IsFormValid();
        _playButton.Disabled = !IsFormValid();
    }

    private void BuildLoadingOverlay()
    {
        _loadingLayer = new CanvasLayer
        {
            Name = "LoadingLayer",
            Layer = 2,
        };
        AddChild(_loadingLayer);

        _loadingRoot = new Control
        {
            Name = "LoadingRoot",
            Visible = false,
            MouseFilter = Control.MouseFilterEnum.Stop,
        };
        _loadingRoot.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        _loadingLayer.AddChild(_loadingRoot);

        var blurOverlay = new ColorRect
        {
            Name = "BlurOverlay",
            Color = new Color(0f, 0f, 0f, 0.35f),
            MouseFilter = Control.MouseFilterEnum.Stop,
        };
        blurOverlay.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        _loadingRoot.AddChild(blurOverlay);

        var spinnerContainer = new CenterContainer
        {
            Name = "SpinnerContainer",
            MouseFilter = Control.MouseFilterEnum.Ignore,
        };
        spinnerContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        _loadingRoot.AddChild(spinnerContainer);

        _spinnerLabel = new Label
        {
            Name = "SpinnerLabel",
            Text = "Loading...",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        _spinnerLabel.AddThemeFontSizeOverride("font_size", 28);
        spinnerContainer.AddChild(_spinnerLabel);
    }

    private void SetLoadingOverlayVisible(bool visible)
    {
        _loadingRoot.Visible = visible;
    }

    private void OnGeneratePressed()
    {
        if (!IsFormValid()) return;
        _ = RegenerateMapAsync();
    }

    private void OnPlayPressed()
    {
        if (!IsFormValid()) return;
        _navigator.NavigateTo(RouteNames.MainMenu);
    }
}