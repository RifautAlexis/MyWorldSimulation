using System;
using Colony.Engine.Simulation;
using Colony.Godot.Scripts.Infrastructure.DependencyInjection;
using Colony.Godot.Scripts.Screens;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace Colony.Godot.Scripts.Infrastructure;

public class ScreenFactory
{
    private readonly IServiceProvider _services;
    private readonly ColonySimulationFactory _simulationFactory;

    public ScreenFactory(IServiceProvider services, ColonySimulationFactory simulationFactory)
    {
        _services = services;
        _simulationFactory = simulationFactory;
    }

    public T CreateScreen<T>() where T : Node
    {
        var screen = _services.GetRequiredService<T>();

        DependencyInjector.Inject(screen, _services);

        return screen;
    }

    public ColonySimulation CreateNewGame()
    {
        var settings = new SimulationSettings
        {
            TicksPerSecond = 10,
            TicksPerGameMinute = 10,
            MinutesPerGameHour = 60,
            HoursPerGameDay = 24,
            SpeedMultiplier = 1.0,
        };

        var simulation = _simulationFactory.CreateNewGame(settings);
        simulation.Start();

        return simulation;
    }

    public WorldScreen CreateWorldScreen(ColonySimulation simulation)
    {
        var worldScreen = new WorldScreen();
        worldScreen.Initialize(simulation);

        DependencyInjector.Inject(worldScreen, _services);

        return worldScreen;
    }
}