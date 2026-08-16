using Colony.Engine.Entities;
using Colony.Engine.Generation;
using Colony.Engine.Simulation.Systems;
using Colony.Engine.Simulation.Views;
using Colony.Engine.World;

namespace Colony.Engine.Simulation;

internal sealed class SimulationEngine
{
    private readonly SimulationClock _clock;
    private readonly SimulationData _data;
    private readonly SimulationSpeed _speed;
    private readonly IReadOnlyCollection<ISimulationSystem> _systems;

    public long TickNumber { get; private set; }

    internal SimulationState State { get; }

    public Grid World => _data.World;
    public GameTime GameTime => _data.GameTime;

    public SimulationEngine(SimulationClock clock,
                            GameTime gameTime,
                            SimulationSpeed speed,
                            Grid world,
                            PopulationData populationData,
                            PopulationSeeder populationSeeder,
                            IEnumerable<ISimulationSystem> systems)
    {
        _clock = clock;
        _speed = speed;
        _data = new SimulationData(world, gameTime, populationData);

        State = SimulationState.Created;

        _systems = systems
                  .OrderBy(s => s.SimulationPhase)
                  .ThenBy(s => s.ExecutionOrder)
                  .ToArray();

        populationSeeder.SeedInitialPopulation(_data.Population);
    }

    public void Update(double deltaTime)
    {
        var adjustedDelta = deltaTime * _speed.Multiplier;

        var ticks = _clock.Update(adjustedDelta);

        for (var i = 0; i < ticks; i++) ExecuteTick();
    }

    private void ExecuteTick()
    {
        TickNumber++;

        var context = new SimulationContext(TickNumber, GameTime, _data);

        foreach (var system in _systems) system.Tick(context);
    }

    public void SetSpeed(double multiplier)
    {
        _speed.SetMultiplier(multiplier);
    }

    internal IReadOnlyList<ColonistView> GetColonistViews()
    {
        return _data.Population.Colonists
                    .Select(colonist => new ColonistView(
                                colonist.Id,
                                colonist.Position.X,
                                colonist.Position.Layer,
                                colonist.Position.Y))
                    .ToArray();
    }
}