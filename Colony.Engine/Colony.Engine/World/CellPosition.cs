namespace Colony.Engine.World;

public readonly record struct CellPosition
{
    public int X { get; }
    public int Y { get; }
    public int Layer { get; }

    public CellPosition(int x, int y, int layer)
    {
        X = x;
        Y = y;
        Layer = layer;
    }
}