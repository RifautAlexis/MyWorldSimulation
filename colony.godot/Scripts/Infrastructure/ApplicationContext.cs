using Colony.Godot.Scripts.Screens;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace Colony.Godot.Scripts.Infrastructure;

public class ApplicationContext
{
    private readonly ServiceProvider _services;
    private readonly ScreenNavigator _screenNavigator;
    private readonly SceneManager _sceneManager;

    public ApplicationContext()
    {
        _services = ServiceConfiguration.Build();
        
        _sceneManager = _services.GetRequiredService<SceneManager>();
        _screenNavigator = _services.GetRequiredService<ScreenNavigator>();
    }
    
    public void Initialize(Node root)
    {
        var screenHost = new ScreenHost();

        root.AddChild(screenHost);

        _sceneManager.Initialize(screenHost);

        _screenNavigator.Navigate<MainMenu>();
    }
}