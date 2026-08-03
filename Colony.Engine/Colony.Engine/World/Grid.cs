namespace Colony.Engine.World;

public class Grid
{
    private readonly TerrainGenerator _terrainGenerator;
    
    private readonly Dictionary<CellPosition, Cell> _cells = new();

    public int Width { get; }
    public int Height { get; }
    public int LayerCount { get; }

    public Grid(int width, int height, int layerCount,  TerrainGenerator terrainGenerator)
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
        if(TryGetCell(position, out var cell))
        {
            return cell;
        }
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
}