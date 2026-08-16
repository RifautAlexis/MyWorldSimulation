using Colony.Engine.Entities;
using Colony.Engine.World;

namespace Colony.Engine.Generation;

internal sealed class PopulationSeeder
{
    public void SeedInitialPopulation(PopulationData populationData)
    {
        ArgumentNullException.ThrowIfNull(populationData);

        var colonist = new Colonist(
            1,
            new CellPosition(0, 3, 0));

        populationData.AddColonist(colonist);
    }
}