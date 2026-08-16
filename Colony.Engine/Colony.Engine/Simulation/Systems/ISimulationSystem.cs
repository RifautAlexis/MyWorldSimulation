namespace Colony.Engine.Simulation.Systems;

internal interface ISimulationSystem
{
    SimulationPhase SimulationPhase { get; }
    int ExecutionOrder { get; }

    void Tick(SimulationContext context);
}