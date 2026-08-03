using Godot;
using Colony.Godot.Scripts.Infrastructure;
using Colony.Godot.Scripts.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Colony.Godot.Scripts.Bootstrap;

public partial class Application : Node
{
    private ApplicationContext? _context;
    private ServiceProvider? _services;

    public override void _Ready()
    {
        CreateContext();
        CallDeferred(nameof(StartApplication));
    }

    private void StartApplication()
    {
        if (_context == null)
            CreateContext();

        _context.Initialize(this);
    }

    public override void _ExitTree()
    {
        _services?.Dispose();
        _services = null;
    }

    private void CreateContext()
    {
        _services = ServiceConfiguration.Build();
        _context = new ApplicationContext();
        DependencyInjector.Inject(_context, _services);
    }
}