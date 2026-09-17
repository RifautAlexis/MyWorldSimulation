namespace Colony.Engine.Facade.Contracts;

public class MapGenerationSettings
{
    public required int XAxisSize { get; set; }
    public required int YAxisSize { get; set; }
    public required int LayerCount { get; init; } = 100;
    public required string Seed { get; set; }
}