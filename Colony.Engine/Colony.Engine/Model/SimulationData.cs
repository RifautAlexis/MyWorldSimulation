using Colony.Engine.Entities;
using Colony.Engine.World;

namespace Colony.Engine.Simulation;

internal sealed class SimulationData
{
    public Grid World { get; }
    public GameTime GameTime { get; }
    public PopulationData Population { get; }

    public SimulationData(Grid world,
                          GameTime gameTime,
                          PopulationData population)
    {
        World = world;
        GameTime = gameTime;
        Population = population;
    }
}