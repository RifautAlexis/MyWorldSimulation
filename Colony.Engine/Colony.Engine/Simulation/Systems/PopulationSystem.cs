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
        var possibleDirections = new List<int> { 0, 1, 2, 3 }; // Up, Right, Down, Left
        possibleDirections.RemoveAt(colonist.Direction);

        var currentPosition = colonist.Position;
        var nextPosition = new CellPosition(
            currentPosition.X,
            currentPosition.Y,
            currentPosition.Layer);
        switch (colonist.Direction)
        {
            case 0:
                nextPosition =
                    new CellPosition(colonist.Position.X, colonist.Position.Y - 1, colonist.Position.Layer);
                break;
            case 1:
                nextPosition =
                    new CellPosition(colonist.Position.X + 1, colonist.Position.Y, colonist.Position.Layer);
                break;
            case 2:
                nextPosition =
                    new CellPosition(colonist.Position.X, colonist.Position.Y + 1, colonist.Position.Layer);
                break;
            case 3:
                nextPosition =
                    new CellPosition(colonist.Position.X - 1, colonist.Position.Y, colonist.Position.Layer);
                break;
        }

        for (var i = 0; i < 5; i++)
        {
            Console.WriteLine(
                $"Colonist {colonist.Id} checking move from ({currentPosition.X}, {currentPosition.Y}, {currentPosition.Layer}) to ({nextPosition.X}, {nextPosition.Y}, {nextPosition.Layer}) in direction {colonist.Direction}");
            if (context.Data.World.CanMove(colonist.Position, nextPosition))
                break;

            var random = new Random();
            var index = random.Next(possibleDirections.Count);
            var direction = possibleDirections[index];
            possibleDirections.Remove(direction);
            colonist.SetDirection(direction); // Change direction to the new random direction

            nextPosition = new CellPosition(
                currentPosition.X,
                currentPosition.Y,
                currentPosition.Layer);

            switch (colonist.Direction)
            {
                case 0:
                    nextPosition =
                        new CellPosition(colonist.Position.X, colonist.Position.Y - 1, colonist.Position.Layer);
                    break;
                case 1:
                    nextPosition =
                        new CellPosition(colonist.Position.X + 1, colonist.Position.Y, colonist.Position.Layer);
                    break;
                case 2:
                    nextPosition =
                        new CellPosition(colonist.Position.X, colonist.Position.Y + 1, colonist.Position.Layer);
                    break;
                case 3:
                    nextPosition =
                        new CellPosition(colonist.Position.X - 1, colonist.Position.Y, colonist.Position.Layer);
                    break;
                default:
                    Console.WriteLine($"All directions invalid for colonist {colonist.Id}");
                    return;
            }
        }

        Console.WriteLine(
            $"Valid direction {colonist.Direction} for colonist {colonist.Id} moving from ({currentPosition.X}, {currentPosition.Y}, {currentPosition.Layer}) to ({nextPosition.X}, {nextPosition.Y}, {nextPosition.Layer})");
        colonist.MoveTo(nextPosition);
    }
}