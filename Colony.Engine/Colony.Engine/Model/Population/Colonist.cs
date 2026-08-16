using Colony.Engine.World;

namespace Colony.Engine.Entities;

internal sealed class Colonist
{
    public int Id { get; }
    public CellPosition Position { get; private set; }
    public int Direction { get; private set; } // Direction the colonist is facing (0-7 for 8 directions)

    public Colonist(int id, CellPosition position)
    {
        Id = id;
        Position = position;
        Direction = 1; // Default direction
    }

    public void MoveTo(CellPosition newPosition)
    {
        Position = newPosition;
    }

    public void ReverseDirection()
    {
        Direction *= -1;
    }
}