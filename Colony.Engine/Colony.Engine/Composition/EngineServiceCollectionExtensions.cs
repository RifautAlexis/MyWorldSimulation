using Colony.Engine.Entities;
using Colony.Engine.Generation;
using Colony.Engine.Simulation;
using Colony.Engine.Simulation.Systems;
using Colony.Engine.World;
using Microsoft.Extensions.DependencyInjection;

namespace Colony.Engine.Infrastructure;

public static class EngineServiceCollectionExtensions
{
    public static IServiceCollection AddColonyEngine(this IServiceCollection services)
    {
        services.AddSingleton<ColonySimulationFactory>();

        return services;
    }

    internal static IServiceCollection AddColonySimulationServices(this IServiceCollection services)
    {
        services.AddSingleton<WorldFactory>();
        services.AddSingleton<PopulationSeeder>();

        services.AddSingleton<Grid>(provider => provider.GetRequiredService<WorldFactory>().CreateDefaultWorld());

        services.AddSingleton<PopulationData>();

        services.AddSingleton<SimulationClock>();
        services.AddSingleton<GameTime>();
        services.AddSingleton<SimulationSpeed>();

        // Below add the simulation systems to the DI container.
        // Each system is responsible for a specific aspect of the simulation, such as managing game time,
        // handling events, or updating the world state. By registering these systems as scoped services,
        // we ensure that each simulation instance has its own set of systems that can be used during the simulation lifecycle.
        services.AddSingleton<ISimulationSystem, GameTimeSystem>();
        services.AddSingleton<ISimulationSystem, PopulationSystem>();

        services.AddSingleton<SimulationEngine>();

        return services;
    }
}