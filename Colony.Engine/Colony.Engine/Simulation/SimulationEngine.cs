using Colony.Engine.Simulation.Systems;
using Colony.Engine.World;

namespace Colony.Engine.Simulation;

internal sealed class SimulationEngine
{
    private readonly SimulationClock _clock;
    private readonly SimulationSpeed _speed;
    private readonly IReadOnlyCollection<ISimulationSystem> _systems;

    public Grid World { get; }
    public GameTime GameTime { get; }
    public long TickNumber { get; private set; }

    public SimulationEngine(SimulationClock clock,
                            GameTime gameTime,
                            SimulationSpeed speed,
                            IEnumerable<ISimulationSystem> systems)
    {
        _clock = clock;
        GameTime = gameTime;
        _speed = speed;

        _systems = systems.ToArray();

        var configuration = new WorldConfiguration
        {
            Width = 10,
            Height = 10,
            LayerCount = 3,
        };

        var terrainGenerator = new TerrainGenerator();

        World = new Grid(configuration.Width, configuration.Height, configuration.LayerCount, terrainGenerator);
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

        var context = new SimulationContext(TickNumber, GameTime);

        foreach (var system in _systems) system.Tick(context);
    }

    public void SetSpeed(double multiplier)
    {
        _speed.SetMultiplier(multiplier);
    }
}