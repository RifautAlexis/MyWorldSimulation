using Colony.Engine.World;

namespace Colony.Engine.Entities;

internal sealed class Colonist
{
    public int Id { get; }
    public CellPosition Position { get; private set; }
    public int Direction { get; private set; } // Direction the colonist is facing (0-3 for 4 directions)

    public Colonist(int id, CellPosition position, int direction)
    {
        Id = id;
        Position = position;
        Direction = direction;
    }

    public void MoveTo(CellPosition newPosition)
    {
        Position = newPosition;
    }

    public void ReverseDirection()
    {
        Direction = (Direction + 2) % 4;
    }

    public void SetDirection(int direction)
    {
        Direction = direction;
    }
}