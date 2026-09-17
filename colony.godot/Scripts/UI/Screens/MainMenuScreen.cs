using Colony.Godot.Scripts.Infrastructure.Navigation;
using Godot;

namespace Colony.Godot.Scripts.UI.Screens;

public sealed partial class MainMenuScreen(IScreenNavigator navigator) : BaseScreen(navigator)
{
    private Control _uiRoot = null!;

    public override void _Ready()
    {
        _uiRoot = new Control();
        _uiRoot.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        AddChild(_uiRoot);
        VisualRoot = _uiRoot;

        BuildUi();
    }

    private void BuildUi()
    {
        var title = new Label
        {
            Text = "Colony",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Position = new Vector2(0, 120),
        };
        title.SetAnchorsPreset(Control.LayoutPreset.TopWide);
        _uiRoot.AddChild(title);

        var layout = new VBoxContainer
        {
            CustomMinimumSize = new Vector2(220, 52),
        };
        layout.SetAnchorsPreset(Control.LayoutPreset.Center);
        _uiRoot.AddChild(layout);

        var startButton = new Button
        {
            Text = "Start game",
            CustomMinimumSize = new Vector2(220, 52),
        };
        startButton.Pressed += () => _navigator.NavigateTo(RouteNames.MapGenerationSetup);
        layout.AddChild(startButton);

        var exitButton = new Button
        {
            Text = "Exit",
            CustomMinimumSize = new Vector2(220, 52),
        };
        exitButton.Pressed += () => GetTree().Quit();
        layout.AddChild(exitButton);
    }
}