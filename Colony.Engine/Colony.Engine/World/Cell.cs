namespace Colony.Engine.World;

public class Cell
{
    public CellPosition Position { get; }
    public TerrainType TerrainType { get; }

    public Cell(CellPosition position, TerrainType terrainType)
    {
        Position = position;
        TerrainType = terrainType;
    }
}