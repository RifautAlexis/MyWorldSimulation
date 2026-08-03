using Colony.Engine.World;

namespace Colony.Engine.Simulation;

public sealed class SimulationEngine
{
    public Grid World { get; }

    public SimulationEngine()
    {
        var configuration = new WorldConfiguration
        {
            Width = 10,
            Height = 10,
            LayerCount = 3,
        };
        
        var terrainGenerator = new TerrainGenerator();

        World = new Grid(configuration.Width, configuration.Height, configuration.LayerCount, terrainGenerator);
    }
}