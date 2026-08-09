namespace Colony.Engine.Simulation;

public sealed class SimulationSettings
{
    public int TicksPerSecond { get; init; } = 10;

    public int TicksPerGameMinute { get; init; } = 10;

    public int MinutesPerGameHour { get; init; } = 60;

    public int HoursPerGameDay { get; init; } = 24;

    public double SpeedMultiplier { get; init; } = 1.0;
}