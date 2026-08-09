using Colony.Engine.World;

namespace Colony.Engine.Simulation;

public sealed class ColonySimulation
{
    private readonly SimulationEngine _engine;

    public Grid World => _engine.World;
    public GameTime GameTime => _engine.GameTime;

    public bool IsRunning => State == SimulationState.Running;
    public bool IsPaused => State == SimulationState.Paused;

    public SimulationState State { get; private set; } = SimulationState.Stopped;

    internal ColonySimulation(SimulationEngine simulationEngine)
    {
        _engine = simulationEngine;

        State = SimulationState.Created;
    }

    public void Start()
    {
        if (State != SimulationState.Created)
            throw new InvalidOperationException($"{nameof(ColonySimulation)} must be created before starting.");

        State = SimulationState.Running;
    }

    public void Tick(double delta)
    {
        if (State != SimulationState.Running)
            return;

        _engine.Update(delta);
    }

    public void SetSpeed(double multiplier)
    {
        _engine.SetSpeed(multiplier);
    }

    public void Pause()
    {
        if (State != SimulationState.Running)
            return;

        State = SimulationState.Paused;
    }

    public void Resume()
    {
        if (State != SimulationState.Paused)
            return;

        State = SimulationState.Running;
    }

    public void Stop()
    {
        if (State == SimulationState.Stopped)
            return;

        State = SimulationState.Stopped;
    }
}