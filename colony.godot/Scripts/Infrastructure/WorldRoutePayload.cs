using Colony.Engine.Simulation;

namespace Colony.Godot.Scripts.Infrastructure;

public sealed class WorldRoutePayload
{
    public required ColonySimulation Simulation { get; init; }
}
