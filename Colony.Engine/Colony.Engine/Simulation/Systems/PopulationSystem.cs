using Colony.Engine.Entities;
using Colony.Engine.World;

namespace Colony.Engine.Simulation.Systems;

internal class PopulationSystem : ISimulationSystem
{
    private const int MovementInterval = 10;

    public SimulationPhase SimulationPhase => SimulationPhase.Actors;
    public int ExecutionOrder => 0;

    public void Tick(SimulationContext context)
    {
        if (context.TickNumber % MovementInterval != 0)
            return;

        foreach (var colonist in context.Data.Population.Colonists) UpdateColonist(colonist, context);
    }

    private void UpdateColonist(Colonist colonist, SimulationContext context)
    {
        var nextX = colonist.Position.X + colonist.Direction;

        var nextPosition = new CellPosition(
            nextX,
            colonist.Position.Y,
            colonist.Position.Layer);

        if (!context.Data.World.CanMove(colonist.Position, nextPosition))
        {
            colonist.ReverseDirection();
            return;
        }

        colonist.MoveTo(nextPosition);
    }
}