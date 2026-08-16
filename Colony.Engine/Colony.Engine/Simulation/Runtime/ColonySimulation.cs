using Colony.Engine.Simulation.Views;
using Colony.Engine.World;
using Microsoft.Extensions.DependencyInjection;

namespace Colony.Engine.Simulation;

public sealed class ColonySimulation : IDisposable
{
    private readonly SimulationEngine _engine;
    private readonly ServiceProvider _serviceProvider;

    public Grid World => _engine.World;
    public GameTime GameTime => _engine.GameTime;
    public long TickNumber => _engine.TickNumber;

    public bool IsRunning => State == SimulationState.Running;
    public bool IsPaused => State == SimulationState.Paused;
    public IReadOnlyList<ColonistView> Colonists => _engine.GetColonistViews();

    public SimulationState State { get; private set; } = SimulationState.Stopped;

    internal ColonySimulation(ServiceProvider serviceProvider, SimulationEngine simulationEngine)
    {
        _serviceProvider = serviceProvider;
        _engine = simulationEngine;

        State = SimulationState.Created;
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
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