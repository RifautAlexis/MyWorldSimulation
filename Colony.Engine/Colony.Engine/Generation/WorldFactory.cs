using Colony.Engine.World;

namespace Colony.Engine.Generation;

internal sealed class WorldFactory
{
    public Grid CreateDefaultWorld(int seed = 12345)
    {
        var configuration = new WorldConfiguration
        {
            Width = 64,
            Height = 64,
            LayerCount = 48,
            Seed = seed,
            MinimumSurfaceLayer = 18,
            MaximumSurfaceLayer = 32,
            MinimumSoilDepth = 2,
            MaximumSoilDepth = 5,
            SurfaceFrequency = 0.05d,
            SurfaceOctaves = 4,
            CaveFrequency = 0.10d,
            CaveThreshold = 0.74d,
            CaveSurfaceBuffer = 3,
        };

        var terrainGenerator = new TerrainGenerator(configuration);

        return new Grid(configuration.Width, configuration.Height, configuration.LayerCount, terrainGenerator);
    }
}