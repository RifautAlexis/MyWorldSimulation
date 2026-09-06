using System;
using Colony.Godot.Scripts.Infrastructure.DependencyInjection;
using Colony.Godot.Scripts.Screens;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace Colony.Godot.Scripts.Infrastructure;

public class ScreenFactory
{
    private readonly IServiceProvider _services;

    public ScreenFactory(IServiceProvider services)
    {
        _services = services;
    }

    public T CreateScreen<T>() where T : Node
    {
        var screen = _services.GetRequiredService<T>();

        DependencyInjector.Inject(screen, _services);

        return screen;
    }

    public WorldScreen CreateWorldScreen(WorldRoutePayload payload)
    {
        var worldScreen = CreateScreen<WorldScreen>();
        worldScreen.Initialize(payload.Simulation);
        return worldScreen;
    }
}