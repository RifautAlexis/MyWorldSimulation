using Colony.Engine.Model.Map;

namespace Colony.Engine.Generation.Models;

public class MapGenerationContext
{
    public MapGenerationSettings GenerationSettings { get; init; }
    public Map Map { get; init; }
}