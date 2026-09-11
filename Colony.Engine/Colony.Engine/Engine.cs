namespace Colony.Engine;

public class Engine
{
    // private Simulation _simulation;
    private MapService _mapService;

    public Engine()
    {
        _mapService = new MapService();
    }
}