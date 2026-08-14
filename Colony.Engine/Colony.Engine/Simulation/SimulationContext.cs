namespace Colony.Engine.Simulation;

internal sealed class SimulationContext
{
    public long TickNumber { get; }
    public GameTime GameTime { get; }

    public SimulationContext(long tickNumber,
                             GameTime gameTime)
    {
        TickNumber = tickNumber;
        GameTime = gameTime;
    }
}