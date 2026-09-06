using Colony.Godot.Scripts.Events;
using Colony.Godot.Scripts.Infrastructure.DependencyInjection;
using Godot;

namespace Colony.Godot.Scripts.Screens.LoadingScreen;

public partial class LoadingScreen : Control,
                                     IInject<IEventBus>
{
    private IEventBus _eventBus = null!;

    public void Inject(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    private void OnGeneratePressed()
    {
        _eventBus.Publish(new NewGameRequested());
    }
}