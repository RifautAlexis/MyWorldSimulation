using Colony.Godot.Scripts.Infrastructure.Navigation;
using Godot;

namespace Colony.Godot.Scripts.UI.Screens;

public partial class LoadingScreen(IScreenNavigator navigator) : BaseScreen(navigator)
{
    private Control _uiRoot = null!;

    public override async void _Ready()
    {
        _uiRoot = new Control();
        _uiRoot.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        AddChild(_uiRoot);
        VisualRoot = _uiRoot;

        BuildUi();

        var timer = GetTree().CreateTimer(5.0);
        await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
        _navigator.NavigateTo(RouteNames.MainMenu);
    }

    private void BuildUi()
    {
        var centerContainer = new CenterContainer
        {
            MouseFilter = Control.MouseFilterEnum.Ignore,
        };

        centerContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);

        _uiRoot.AddChild(centerContainer);

        var titleLabel = BuildTitle("Loading...");
        centerContainer.AddChild(titleLabel);
    }

    private Label BuildTitle(string titleText)
    {
        var titleLabel = new Label
        {
            Text = titleText,
            HorizontalAlignment = HorizontalAlignment.Center,
            CustomMinimumSize = new Vector2(0f, 50f),
        };

        titleLabel.AddThemeFontSizeOverride("font_size", 32);

        return titleLabel;
    }
}