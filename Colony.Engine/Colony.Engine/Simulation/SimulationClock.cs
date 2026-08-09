namespace Colony.Engine.Simulation;

public sealed class SimulationClock
{
    private readonly double _tickInterval;
    private double _accumulator;

    public SimulationClock(double tickPerSecond)
    {
        if (tickPerSecond <= 0)
            throw new ArgumentOutOfRangeException(nameof(tickPerSecond), "Tick per second must be greater than zero.");

        _tickInterval = 1.0 / tickPerSecond;
        _accumulator = 0.0;
    }

    public int Update(double deltaTime)
    {
        _accumulator += deltaTime;

        var ticks = 0;
        
        while (_accumulator >= _tickInterval)
        {
            _accumulator -= _tickInterval;
            ticks++;
        }

        return ticks;
    }
}