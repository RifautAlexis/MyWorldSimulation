namespace Colony.Engine.Simulation.Systems;

internal class GameTimeSystem : ISimulationSystem
{
    private readonly GameTime _gameTime;

    public GameTimeSystem(GameTime gameTime)
    {
        _gameTime = gameTime;
    }

    public int ExecutionOrder => 0;

    public SimulationPhase SimulationPhase => SimulationPhase.Time;

    public void Tick(SimulationContext context)
    {
        _gameTime.Tick();
    }
}