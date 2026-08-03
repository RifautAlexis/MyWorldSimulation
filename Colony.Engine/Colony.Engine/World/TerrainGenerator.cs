namespace Colony.Engine.World;

public sealed class TerrainGenerator
{
    public TerrainType Generate(CellPosition position)
    {
        if (position.X == 0 && position.Y == 0)
        {
            return TerrainType.Water;
        }
        
        return position.Layer switch
        {
            0 => TerrainType.Soil,
            1 or 2 => TerrainType.Rock,
            _ => TerrainType.Air,
        };
    }
}