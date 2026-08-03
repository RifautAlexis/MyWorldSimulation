using Colony.Godot.Scripts.Infrastructure.DependencyInjection;
using Colony.Godot.Scripts.Screens;
using Godot;

namespace Colony.Godot.Scripts.Infrastructure;

public class ApplicationContext : IInject<ScreenNavigator>, IInject<SceneManager>
{
    private ScreenNavigator _screenNavigator = null!;
    private SceneManager _sceneManager = null!;

    public void Inject(ScreenNavigator dependency)
    {
        _screenNavigator = dependency;
    }
    public void Inject(SceneManager dependency)
    {
        _sceneManager = dependency;
    }
    
    public void Initialize(Node root)
    {
        var screenHost = new ScreenHost();

        root.AddChild(screenHost);

        _sceneManager.Initialize(screenHost);

        _screenNavigator.Navigate<MainMenu>();
    }
}