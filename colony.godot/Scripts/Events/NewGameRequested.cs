using Colony.Godot.Scripts.Screens.MapGenerationSetupScreen;

namespace Colony.Godot.Scripts.Events;

public sealed class NewGameRequested
{
    public required MapGenerationSetupForm SetupForm { get; init; }
}