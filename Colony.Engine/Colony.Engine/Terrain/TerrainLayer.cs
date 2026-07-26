namespace Colony.Engine.Terrain;

public class TerrainLayer
{
    private readonly TerrainCell[,] _layer;
    
    public TerrainLayer(int xAxis, int yAxis)
    {
        if (xAxis <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(xAxis), "Map xAxis must be greater than zero.");
        }

        if (yAxis <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(yAxis), "Map yAxis must be greater than zero.");
        }
        
        _layer = GenerateLayer(xAxis, yAxis);
    }

    private static TerrainCell[,] GenerateLayer(int xAxis, int yAxis)
    {
        var layer = new TerrainCell[xAxis, yAxis];

        return layer;
    }
}