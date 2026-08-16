namespace Colony.Engine.Entities;

internal class PopulationData
{
    private readonly List<Colonist> _colonists = new();

    public IReadOnlyList<Colonist> Colonists => _colonists.AsReadOnly();

    public void AddColonist(Colonist colonist)
    {
        ArgumentNullException.ThrowIfNull(colonist);

        _colonists.Add(colonist);
    }

    public void AddColonists(IEnumerable<Colonist> colonists)
    {
        ArgumentNullException.ThrowIfNull(colonists);

        _colonists.AddRange(colonists);
    }
}