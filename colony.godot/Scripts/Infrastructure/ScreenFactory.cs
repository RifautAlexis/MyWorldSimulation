using System;
using Colony.Godot.Scripts.Infrastructure.DependencyInjection;
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
}