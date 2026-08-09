namespace Colony.Engine.Simulation;

internal sealed class SimulationSpeed
{
    public double Multiplier { get; private set; } = 1.0;

    public void SetMultiplier(double multiplier)
    {
        if (multiplier <= 0)
            throw new ArgumentOutOfRangeException(nameof(multiplier), "Multiplier must be greater than zero.");

        Multiplier = multiplier;
    }
}