namespace Colony.Engine.Terrain;

public sealed class Terrain
{
    public int XAxis { get; }

    public int YAxis { get; }

    public int ZAxis { get; }

    private readonly TerrainLayer[] _layers;

    public Terrain(int xAxis, int yAxis, int zAxis)
    {
        Console.WriteLine("Starting Terrain Generation...");
        
        if (xAxis <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(xAxis), "Map xAxis must be greater than zero.");
        }

        if (yAxis <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(yAxis), "Map yAxis must be greater than zero.");
        }

        if (zAxis <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(zAxis), "Map zAxis must be greater than zero.");
        }

        XAxis = xAxis;
        YAxis = yAxis;
        ZAxis = zAxis;

        _layers = GenerateLayers(xAxis, yAxis, zAxis);
        
        Console.WriteLine("Terrain Generation Completed.");
    }

    private static TerrainLayer[] GenerateLayers(int xAxis, int yAxis, int zAxis)
    {
        var layers = Enumerable
            .Range(0, zAxis)
            .Select(_ => new TerrainLayer(xAxis, yAxis))
            .ToArray();

        return layers;
    }
}