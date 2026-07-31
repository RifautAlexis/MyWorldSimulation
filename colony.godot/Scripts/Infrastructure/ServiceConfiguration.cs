using System;
using Colony.Godot.Scripts.Events;
using Colony.Godot.Scripts.Screens;
using Colony.Godot.Scripts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Colony.Godot.Scripts.Infrastructure;

public static class ServiceConfiguration
{
    public static ServiceProvider Build()
    {
        var services = new ServiceCollection();

        RegisterServices(services);
        
        return services.BuildServiceProvider();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        // Infrastructure
        services.AddSingleton<IEventBus, EventBus>();
        services.AddSingleton<SceneManager>();
        services.AddSingleton<ScreenFactory>();
        services.AddSingleton<ScreenNavigator>();
        
        // Screens
        services.AddTransient<MainMenu>();
        services.AddTransient<World>();

        // Controller
        services.AddSingleton<CameraController>();
    }
}