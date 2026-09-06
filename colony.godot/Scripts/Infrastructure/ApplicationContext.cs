using Colony.Godot.Scripts.Infrastructure.DependencyInjection;
using Godot;

namespace Colony.Godot.Scripts.Infrastructure;

public class ApplicationContext : IInject<ScreenNavigator>,
                                  IInject<SceneManager>
{
    private SceneManager _sceneManager = null!;
    private ScreenNavigator _screenNavigator = null!;

    public void Inject(SceneManager dependency)
    {
        _sceneManager = dependency;
    }

    public void Inject(ScreenNavigator dependency)
    {
        _screenNavigator = dependency;
    }

    public void Initialize(Node root)
    {
        var screenHost = new ScreenHost();

        root.AddChild(screenHost);

        _sceneManager.Initialize(screenHost);

        _screenNavigator.NavigateTo(AppRoute.MainMenu);
    }
}