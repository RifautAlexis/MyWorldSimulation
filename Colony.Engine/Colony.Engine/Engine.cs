namespace Colony.Engine.Simulation;

public sealed class Engine
{
    public Terrain.Terrain Terrain { get; private set; }
    
    public void Initialize()
    {
        Console.WriteLine("Starting Colony.Engine...");
        
        Terrain = new Terrain.Terrain(64, 64, 10);
        
        Console.WriteLine("Colony.Engine initialized.");
    }
    
    public void Tick()
    {
        Console.WriteLine("Ticking Colony.Engine...");
    }
}