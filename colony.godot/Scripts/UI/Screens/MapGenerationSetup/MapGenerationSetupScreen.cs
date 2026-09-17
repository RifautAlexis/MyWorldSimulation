using System;
using System.Threading;
using System.Threading.Tasks;
using Colony.Engine.Facade;
using Colony.Engine.Facade.Contracts;
using Colony.Godot.Scripts.Infrastructure.Navigation;
using Colony.Godot.Scripts.UI.Controllers;
using Colony.Godot.Scripts.UI.Renderers;
using Colony.Godot.Scripts.UI.Screens.MapGenerationSetup.UI;
using Godot;

namespace Colony.Godot.Scripts.UI.Screens.MapGenerationSetup;

public partial class MapGenerationSetupScreen : BaseScreen
{
    // Constants
    private const int DefaultLayerCount = 100;

    // Layers, Roots and Containers
    private Node3D _mapRoot = null!;
    private CanvasLayer _hudLayer = null!;
    private MapSetupForm _mapSetupForm = null!;
    private LoadingOverlay _loadingOverlay = null!;
    private CanvasLayer _loadingLayer = null!;
    private Node3D _mapContainer = null!;

    // Dependencies
    private readonly IScreenNavigator _navigator = null!;
    private readonly MapRenderer _mapRenderer = null!;
    private readonly CameraController _cameraController = null!;

    // State
    private MapGenerationSetupState _state = null!;

    // Form and Generation
    private CancellationTokenSource? _generationCts;

    public MapGenerationSetupScreen(IScreenNavigator navigator) : base(navigator)
    {
        _navigator = navigator;

        var layerRenderer = new LayerRenderer();
        _mapRenderer = new MapRenderer(layerRenderer);
        _cameraController = new CameraController();

        InitializeState();
    }

    public override void _Ready()
    {
        BuildMap();
        BuildHud();
        BuildLoadingOverlay();

        _ = RegenerateMapAsync();
    }

    public override void _Process(double delta)
    {
        _cameraController.Update((float)delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        _cameraController.HandleInput(@event);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (_mapSetupForm.GetHudRoot() != null) _mapSetupForm.Show();
    }

    public override void OnExit()
    {
        base.OnExit();
        if (_mapSetupForm.GetHudRoot() != null) _mapSetupForm.Hide();
    }

    private void BuildMap()
    {
        _mapRoot = new Node3D
        {
            Name = "MapRoot",
        };
        AddChild(_mapRoot);
        VisualRoot = _mapRoot;

        _mapContainer = new Node3D
        {
            Name = "MapContainer",
        };
        _mapRoot.AddChild(_mapContainer);

        CreateLight();
        BuildCamera(_state.Form.MapSize.XAxis, _state.Form.MapSize.YAxis);
    }

    private void BuildHud()
    {
        _hudLayer = new CanvasLayer
        {
            Name = "HudLayer",
            Layer = 1,
        };
        AddChild(_hudLayer);

        _mapSetupForm = new MapSetupForm();
        var hudRoot = _mapSetupForm.BuildMapSetupForm(CreateBindings());
        _hudLayer.AddChild(hudRoot);
    }

    private void BuildLoadingOverlay()
    {
        _loadingLayer = new CanvasLayer
        {
            Name = "LoadingLayer",
            Layer = 2, // above HUD
        };
        AddChild(_loadingLayer);

        _loadingOverlay = new LoadingOverlay();
        var loadingRoot = _loadingOverlay.BuildLoadingOverlay();
        _loadingLayer.AddChild(loadingRoot);
    }

    private void InitializeState()
    {
        _state = new MapGenerationSetupState();

        _state.FormChanged += RefreshUiState;
        _state.IsGeneratingChanged += OnIsGeneratingChanged;
    }

    private void RefreshUiState()
    {
        _mapSetupForm.SetGenerateButtonEnabled(IsFormValid() && !_state.IsGenerating);
        _mapSetupForm.SetPlayButtonEnabled(IsFormValid() && !_state.IsGenerating);
    }

    private void OnIsGeneratingChanged()
    {
        RefreshUiState();

        SetLoadingOverlayVisible(_state.IsGenerating);
    }

    private void BuildCamera(int xAxisSize, int yAxisSize)
    {
        var initialCenter = new Vector3(xAxisSize / 2, 0, yAxisSize / 2);

        var cameraPivot = new Node3D
        {
            Name = "CameraPivot",
        };

        _mapRoot.AddChild(cameraPivot);

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

        _mapRoot.AddChild(light);
    }

    private async Task RegenerateMapAsync()
    {
        if (_state.IsGenerating)
            return;

        _state.SetIsGenerating(true);
        _generationCts?.Cancel();
        _generationCts?.Dispose();
        _generationCts = new CancellationTokenSource();

        SetLoadingOverlayVisible(true);

        var progress = new Progress<MapGenerationProgress>(OnMapGenerationProgress);

        try
        {
            IColonyEngine engine = new ColonyEngineFacade();

            var mapSettings = new MapGenerationSettings
            {
                XAxisSize = _state.Form.MapSize.XAxis,
                YAxisSize = _state.Form.MapSize.YAxis,
                Seed = _state.Form.Seed,
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
            _loadingOverlay.UpdateCurrentStepLabel("Completed");
        }
        catch (OperationCanceledException)
        {
            _loadingOverlay.UpdateCurrentStepLabel("Canceled");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _loadingOverlay.UpdateCurrentStepLabel("Failed");
        }
        finally
        {
            SetLoadingOverlayVisible(false);
            _state.SetIsGenerating(false);
        }
    }

    private void OnMapGenerationProgress(MapGenerationProgress progress)
    {
        _loadingOverlay.UpdateCurrentStepLabel(progress.Step.ToString());
    }

    private bool IsFormValid()
    {
        return _state.Form.MapSize is { XAxis: > 0, YAxis: > 0 } && _state.Form.Seed != null;
    }

    private MapGenerationSetupBindings CreateBindings()
    {
        return new MapGenerationSetupBindings
        {
            InitialMapSize = _state.Form.MapSize,
            InitialSeed = _state.Form.Seed,
            Generate = OnGeneratePressed,
            Play = OnPlayPressed,
            SelectMapSize = mapSize => _state.SetMapSize(mapSize),
            UpdateSeed = value => _state.SetSeed(value),
        };
    }

    private void SetLoadingOverlayVisible(bool visible)
    {
        if (visible)
            _loadingOverlay.Show();
        else
            _loadingOverlay.Hide();
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