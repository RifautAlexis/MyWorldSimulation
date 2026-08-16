namespace Colony.Engine.Simulation;

internal sealed class SimulationContext
{
    public long TickNumber { get; }
    public GameTime GameTime { get; }

    internal SimulationData Data { get; }

    public SimulationContext(long tickNumber,
                             GameTime gameTime,
                             SimulationData data)
    {
        TickNumber = tickNumber;
        GameTime = gameTime;
        Data = data;
    }
}