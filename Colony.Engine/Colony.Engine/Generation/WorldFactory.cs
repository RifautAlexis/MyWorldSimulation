using Colony.Engine.World;

namespace Colony.Engine.Generation;

internal sealed class WorldFactory
{
    public Grid CreateDefaultWorld()
    {
        var configuration = new WorldConfiguration
        {
            Width = 10,
            Height = 10,
            LayerCount = 3,
        };

        var terrainGenerator = new TerrainGenerator();

        return new Grid(configuration.Width, configuration.Height, configuration.LayerCount, terrainGenerator);
    }
}
