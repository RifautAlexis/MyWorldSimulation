namespace Colony.Engine;

public sealed class Simulation
{
    public Guid Id { get; } = Guid.NewGuid();
    
    public World World { get; private set; }
    
    public bool IsRunning { get; private set; }

    public void Start()
    {
        World = new World(64, 64, 100);
        IsRunning = true;
    }

    public void Tick()
    {
        if (!IsRunning)
            return;
    }

    public void Stop()
    {
        IsRunning = false;
        World = null;
    }
}