using Colony.Engine.World;

namespace Colony.Engine.Model.Map;

public class Map
{
    private readonly Dictionary<CellPosition, Cell> _cells = new();
    private MapSettings Settings { get; init; }

    internal Map(MapSettings settings)
    {
        Settings = settings;
    }

    internal void SetCell(CellPosition position, Cell cell)
    {
        _cells[position] = cell;
    }
}