namespace Colony.Engine.Simulation.Systems;

internal enum SimulationPhase
{
    Time = 0,
    World = 100,
    Actors = 200,
    Needs = 300,
    Jobs = 400,
    Economy = 500,
}