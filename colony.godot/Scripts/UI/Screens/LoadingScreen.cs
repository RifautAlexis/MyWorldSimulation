using System;
using System.Threading.Tasks;
using Colony.Godot.Scripts.Infrastructure.Navigation;
using Colony.Godot.Scripts.UI.Screens.MapGenerationSetupScreen;
using Godot;

namespace Colony.Godot.Scripts.UI.Screens.LoadingScreen;

public partial class LoadingScreen : BaseScreen
{
    public LoadingScreen(IScreenNavigator navigator) : base(navigator)
    {
    }

    public override async void _Ready()
    {
        var payload = _navigator.GetPayload<MapGenerationSettings>();
        Console.WriteLine(
            $"LoadingScreen received payload: XAxis={payload?.XAxis}, YAxis={payload?.YAxis}, Seed={payload?.Seed}");
        BuildUi();

        var timer = GetTree().CreateTimer(5.0);
        await ToSignal(timer, SceneTreeTimer.SignalName.Timeout);
        _navigator.NavigateTo(RouteNames.MainMenu);
    }

    private void BuildUi()
    {
        var centerContainer = new CenterContainer
        {
            MouseFilter = MouseFilterEnum.Ignore,
        };

        centerContainer.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);

        AddChild(centerContainer);

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