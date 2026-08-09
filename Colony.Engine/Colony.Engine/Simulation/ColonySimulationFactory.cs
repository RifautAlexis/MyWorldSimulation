using Microsoft.Extensions.DependencyInjection;

namespace Colony.Engine.Simulation;

public class ColonySimulationFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ColonySimulationFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ColonySimulation CreateNewGame(SimulationSettings settings)
    {
        var engine = ActivatorUtilities.CreateInstance<SimulationEngine>(_serviceProvider, settings);

        return new ColonySimulation(engine);
    }
}