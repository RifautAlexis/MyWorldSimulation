namespace Colony.Engine.Simulation.Systems;

internal interface ISimulationSystem
{
    void Tick(SimulationContext context);
}