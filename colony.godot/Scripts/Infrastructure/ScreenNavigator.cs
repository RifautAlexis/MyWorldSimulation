using Colony.Godot.Scripts.Events;
using Colony.Godot.Scripts.Screens;
using Colony.Godot.Scripts.Screens.MapGenerationSetupScreen;
using Godot;

namespace Colony.Godot.Scripts.Infrastructure;

public sealed class ScreenNavigator
{
    private readonly GameSessionBuilder _gameSessionBuilder;
    private readonly SceneManager _sceneManager;
    private readonly ScreenFactory _screenFactory;

    public ScreenNavigator(
        ScreenFactory screenFactory,
        SceneManager sceneManager,
        GameSessionBuilder gameSessionBuilder,
        IEventBus eventBus)
    {
        _screenFactory = screenFactory;
        _sceneManager = sceneManager;
        _gameSessionBuilder = gameSessionBuilder;

        eventBus.Subscribe<MainMenuRequested>(_ => NavigateTo(AppRoute.MainMenu));
        eventBus.Subscribe<MapGenerationSetupRequested>(OnMapGenerationSetupRequested);
        eventBus.Subscribe<NewGameRequested>(OnNewGameRequested);
    }

    public void NavigateTo(AppRoute route, object? payload = null)
    {
        var screen = route switch
        {
            AppRoute.MainMenu => _screenFactory.CreateScreen<MainMenuScreen>(),
            AppRoute.MapGenerationSetup => _screenFactory.CreateScreen<MapGenerationSetupScreen>(),
            AppRoute.World => payload is WorldRoutePayload worldPayload
                ? _screenFactory.CreateWorldScreen(worldPayload)
                : throw new InvalidOperationException("World route requires WorldRoutePayload."),
            _ => throw new ArgumentOutOfRangeException(nameof(route), route, "Unsupported route."),
        };

        _sceneManager.Show(screen);
    }

    private void OnMapGenerationSetupRequested(MapGenerationSetupRequested _)
    {
        NavigateTo(AppRoute.MapGenerationSetup);
    }

    private void OnNewGameRequested(NewGameRequested @event)
    {
        var worldPayload = _gameSessionBuilder.BuildWorldPayload(@event.SetupForm);
        NavigateTo(AppRoute.World, worldPayload);
    }
}