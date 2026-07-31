using Colony.Godot.Scripts.Events;
using Colony.Godot.Scripts.Screens;
using Godot;

namespace Colony.Godot.Scripts.Infrastructure;

public sealed class ScreenNavigator
{
    private readonly ScreenFactory _screenFactory;
    private readonly SceneManager _sceneManager;
    
    public ScreenNavigator(ScreenFactory screenFactory, SceneManager sceneManager, IEventBus eventBus)
    {
        _screenFactory = screenFactory;
        _sceneManager = sceneManager;
        
        eventBus.Subscribe<NewGameRequested>(OnNewGameRequested);
    }

    public void Navigate<T>() where T : Node
    {
        var screen = _screenFactory.CreateScreen<T>();
        _sceneManager.Show(screen);
    }
    
    private void OnNewGameRequested(NewGameRequested _)
    {
        Navigate<World>();
    }
}