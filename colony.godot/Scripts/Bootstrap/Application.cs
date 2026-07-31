using Godot;
using Colony.Godot.Scripts.Infrastructure;

namespace Colony.Godot.Scripts.Bootstrap;

public partial class Application : Node
{
    private ApplicationContext? _context;

    public override void _Ready()
    {
        _context = new ApplicationContext();
        
        CallDeferred(nameof(StartApplication));
    }

    private void StartApplication()
    {
        if(_context == null)
            _context = new ApplicationContext();
        
        _context.Initialize(this);
    }
}