namespace Colony.Engine.World;

public class Grid
{
    private readonly Dictionary<CellPosition, Cell> _cells = new();
    private readonly TerrainGenerator _terrainGenerator;

    public int Width { get; }
    public int Height { get; }
    public int LayerCount { get; }

    public Grid(int width, int height, int layerCount, TerrainGenerator terrainGenerator)
    {
        Width = width;
        Height = height;
        LayerCount = layerCount;
        _terrainGenerator = terrainGenerator;

        CreateCells(_terrainGenerator);
    }

    public IEnumerable<Cell> GetCells()
    {
        return _cells.Values;
    }

    public Cell? GetCell(CellPosition position)
    {
        if (TryGetCell(position, out var cell)) return cell;

        return null;
    }

    public bool TryGetCell(CellPosition position, out Cell? cell)
    {
        return _cells.TryGetValue(position, out cell);
    }

    public bool Contains(CellPosition position)
    {
        return
            position.X >= 0 &&
            position.X < Width &&
            position.Y >= 0 &&
            position.Y < Height &&
            position.Layer >= 0 &&
            position.Layer < LayerCount;
    }

    public IEnumerable<Cell> GetLayer(int layer)
    {
        if (layer < 0 || layer >= LayerCount)
            throw new ArgumentOutOfRangeException(nameof(layer), "Layer is out of range.");

        return _cells.Values.Where(cell => cell.Position.Layer == layer);
    }

    public bool IsWalkable(CellPosition position)
    {
        if (!IsInside(position)) return false;

        var cell = GetCell(position);

        if (cell is null) return false;
        return cell.TerrainType is
            TerrainType.Soil or
            TerrainType.Rock;
    }

    public bool CanMove(CellPosition from, CellPosition to)
    {
        if (!IsInside(from)) return false;

        if (!IsInside(to)) return false;

        if (!IsWalkable(to)) return false;

        // More rules later...

        return true;
    }

    private void CreateCells(TerrainGenerator terrainGenerator)
    {
        for (var layer = 0; layer < LayerCount; layer++)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var position = new CellPosition(x, y, layer);
                    var terrain = terrainGenerator.Generate(position);

                    _cells.Add(position, new Cell(position, terrain));
                }
            }
        }
    }

    private bool IsInside(CellPosition position)
    {
        return position.X >= 0 &&
               position.X < Width &&
               position.Y >= 0 &&
               position.Y < Height &&
               position.Layer >= 0 &&
               position.Layer < LayerCount;
    }
}