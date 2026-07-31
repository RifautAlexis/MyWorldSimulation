namespace Colony.Engine;

public class WorldCell
{
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public WorldCell(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
}