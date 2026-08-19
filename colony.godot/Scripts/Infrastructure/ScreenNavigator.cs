using Colony.Godot.Scripts.Events;
using Colony.Godot.Scripts.Screens;
using Godot;

namespace Colony.Godot.Scripts.Infrastructure;

public sealed class ScreenNavigator
{
    private readonly SceneManager _sceneManager;
    private readonly ScreenFactory _screenFactory;

    public ScreenNavigator(ScreenFactory screenFactory, SceneManager sceneManager, IEventBus eventBus)
    {
        _screenFactory = screenFactory;
        _sceneManager = sceneManager;

        eventBus.Subscribe<NewGameRequested>(OnNewGameRequested);
        eventBus.Subscribe<MainMenuRequested>(_ => Navigate<MainMenuScreen>());
    }

    public void Navigate<T>() where T : Node
    {
        var screen = _screenFactory.CreateScreen<T>();
        _sceneManager.Show(screen);
    }

    private void OnNewGameRequested(NewGameRequested _)
    {
        var simulation = _screenFactory.CreateNewGame();
        var worldScreen = _screenFactory.CreateWorldScreen(simulation);
        _sceneManager.Show(worldScreen);
    }
}