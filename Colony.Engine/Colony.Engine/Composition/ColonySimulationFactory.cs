using Colony.Engine.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Colony.Engine.Simulation;

public sealed class ColonySimulationFactory
{
    public ColonySimulation CreateNewGame(SimulationSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var services = new ServiceCollection();

        services.AddSingleton(settings);

        services.AddColonySimulationServices();

        var serviceProvider = services.BuildServiceProvider();

        var engine = serviceProvider.GetRequiredService<SimulationEngine>();

        return new ColonySimulation(serviceProvider, engine);
    }

    public Engine CreateNewEngine()
    {
        return new Engine();
    }
}