using Colony.Engine.Entities;
using Colony.Engine.World;

namespace Colony.Engine.Generation;

internal sealed class PopulationSeeder
{
    public void SeedInitialPopulation(PopulationData populationData)
    {
        ArgumentNullException.ThrowIfNull(populationData);

        var colonists = new List<Colonist>
        {
            new(
                1,
                new CellPosition(0, 0, 0),
                0),
            new(
                2,
                new CellPosition(1, 1, 0),
                1),
            new(
                3,
                new CellPosition(2, 2, 0),
                2),
            new(
                4,
                new CellPosition(3, 3, 0),
                3),
            new(
                5,
                new CellPosition(4, 4, 0),
                3),
            new(
                6,
                new CellPosition(5, 5, 0),
                2),
            new(
                7,
                new CellPosition(6, 6, 0),
                1),
            new(
                8,
                new CellPosition(7, 7, 0),
                0),
        };

        populationData.AddColonists(colonists);
    }
}