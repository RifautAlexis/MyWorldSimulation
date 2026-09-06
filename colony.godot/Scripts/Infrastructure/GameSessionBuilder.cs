using Colony.Engine.Simulation;
using Colony.Godot.Scripts.Screens.MapGenerationSetupScreen;

namespace Colony.Godot.Scripts.Infrastructure;

public sealed class GameSessionBuilder
{
    private readonly ColonySimulationFactory _simulationFactory;

    public GameSessionBuilder(ColonySimulationFactory simulationFactory)
    {
        _simulationFactory = simulationFactory;
    }

    public WorldRoutePayload BuildWorldPayload(MapGenerationSetupForm setupForm)
    {
        var settings = new SimulationSettings
        {
            Width = setupForm.MapSize.XAxis,
            Height = setupForm.MapSize.YAxis,
            Seed = setupForm.Seed,
            TicksPerSecond = 10,
            TicksPerGameMinute = 10,
            MinutesPerGameHour = 60,
            HoursPerGameDay = 24,
            SpeedMultiplier = 1.0,
        };

        var simulation = _simulationFactory.CreateNewGame(settings);
        simulation.Start();

        return new WorldRoutePayload
        {
            Simulation = simulation,
        };
    }
}
