using System;
using Colony.Godot.Scripts.Infrastructure.DependencyInjection;
using Colony.Godot.Scripts.Infrastructure.Navigation;
using Colony.Godot.Scripts.UI.Screens;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace Colony.Godot.Scripts.Bootstrap;

public partial class Application : Node
{
    private IScreenNavigator _screenNavigator = null!;
    private ServiceProvider _serviceProvider = null!;

    private Action Routes => () =>
    {
        _screenNavigator.Register(RouteNames.MainMenu, () => new MainMenuScreen(_screenNavigator));
        _screenNavigator.Register(RouteNames.Gameplay, () => new GameplayScreen(_screenNavigator));
    };

    public override void _Ready()
    {
        StartApplication();
    }

    public override void _ExitTree()
    {
        if (_screenNavigator is ScreenNavigator navigator)
            navigator.Clear();

        _serviceProvider?.Dispose();
        _serviceProvider = null;

        _screenNavigator = null!;

        base._ExitTree();
    }

    private void StartApplication()
    {
        var services = new ServiceCollection();
        services.AddColonyApp(this);

        _serviceProvider = services.BuildServiceProvider();
        _screenNavigator = _serviceProvider.GetRequiredService<IScreenNavigator>();

        Routes();

        _screenNavigator.NavigateTo(RouteNames.MainMenu);
    }
}