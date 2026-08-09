using Colony.Engine.Simulation;
using Microsoft.Extensions.DependencyInjection;

namespace Colony.Engine.Infrastructure;

public static class EngineServiceCollectionExtensions
{
    public static IServiceCollection AddColonyEngine(this IServiceCollection services)
    {
        services.AddSingleton<SimulationClock>();
        services.AddSingleton<GameTime>();
        services.AddSingleton<ColonySimulationFactory>();

        return services;
    }
}