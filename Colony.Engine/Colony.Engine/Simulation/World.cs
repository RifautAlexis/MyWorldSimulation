namespace Colony.Engine;

public class World
{
    public int Width { get; }
    public int Height { get; }
    public int Depth { get; }
    
    public World(int width, int height, int depth)
    {
        Width = width;
        Height = height;
        Depth = depth;
    }
}