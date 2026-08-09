using Colony.Engine.World;

namespace Colony.Engine.Simulation;

internal sealed class SimulationEngine
{
    private readonly SimulationClock _clock;
    private readonly SimulationSettings _settings;
    private readonly SimulationSpeed _speed;

    public Grid World { get; }
    public GameTime GameTime { get; }

    public SimulationEngine(SimulationSettings settings)
    {
        _settings = settings;
        _speed = new SimulationSpeed();
        _speed.SetMultiplier(settings.SpeedMultiplier);

        var configuration = new WorldConfiguration
        {
            Width = 10,
            Height = 10,
            LayerCount = 3,
        };

        var terrainGenerator = new TerrainGenerator();

        World = new Grid(configuration.Width, configuration.Height, configuration.LayerCount, terrainGenerator);
        _clock = new SimulationClock(_settings.TicksPerSecond);
        GameTime = new GameTime(_settings);
    }

    public void Update(double deltaTime)
    {
        var adjustedDelta = deltaTime * _speed.Multiplier;

        var ticks = _clock.Update(adjustedDelta);

        for (var i = 0; i < ticks; i++) ExecuteTick();
    }

    private void ExecuteTick()
    {
        GameTime.Tick();
    }

    public void SetSpeed(double multiplier)
    {
        _speed.SetMultiplier(multiplier);
    }
}