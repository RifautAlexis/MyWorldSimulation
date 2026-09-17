using Colony.Godot.Scripts.Infrastructure.Navigation;
using Godot;

namespace Colony.Godot.Scripts.UI.Screens.MainGameScreen;

public sealed partial class MapScreen(IScreenNavigator navigator) : BaseScreen(navigator)
{
    private CanvasLayer _hudLayer = null!;
    private Control _hudRoot = null!;
    private Node3D _worldRoot = null!;

    public override void _Ready()
    {
        BuildWorld();
        BuildHud();
    }

    private void BuildWorld()
    {
        _worldRoot = new Node3D();
        AddChild(_worldRoot);
        VisualRoot = _worldRoot;

        var camera = new Camera3D();
        camera.Current = true;
        camera.Position = new Vector3(0, 20, 20);
        camera.LookAt(Vector3.Zero, Vector3.Up);
        _worldRoot.AddChild(camera);

        var light = new DirectionalLight3D();
        _worldRoot.AddChild(light);

        // Add generated map, units, props, etc.
    }

    private void BuildHud()
    {
        _hudLayer = new CanvasLayer();
        AddChild(_hudLayer);

        _hudRoot = new Control();
        _hudRoot.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        _hudLayer.AddChild(_hudRoot);

        var pauseButton = new Button
        {
            Text = "Pause",
        };
        pauseButton.Position = new Vector2(20, 20);
        _hudRoot.AddChild(pauseButton);
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
}