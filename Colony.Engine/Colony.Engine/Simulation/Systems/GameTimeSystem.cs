namespace Colony.Engine.Simulation.Systems;

internal class GameTimeSystem : ISimulationSystem
{
    private readonly GameTime _gameTime;

    public GameTimeSystem(GameTime gameTime)
    {
        _gameTime = gameTime;
    }

    public void Tick(SimulationContext context)
    {
        _gameTime.Tick();
    }
}