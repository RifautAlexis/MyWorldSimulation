using Colony.Engine.Generation;
using Colony.Engine.Generation.Models;

namespace Colony.Engine;

public class MapService
{
    private readonly MapGenerator _mapGenerator;

    public MapService()
    {
        _mapGenerator = new MapGenerator();
    }

    public async Task GenerateMap(MapGenerationSettings settings)
    {
        Console.WriteLine("Generate a map : TO DO");
        await _mapGenerator.GenerateAsync(settings);
    }
}