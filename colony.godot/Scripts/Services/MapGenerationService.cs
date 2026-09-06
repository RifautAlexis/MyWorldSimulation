using System.Threading;
using System.Threading.Tasks;

namespace Colony.Godot.Scripts.Services;

// public class MapGenerationState
// {
//     public MapGenerationStatus Status { get; init; }
//
//     public MapGenerationStep Step { get; init; }
//
//     public double Progress { get; init; }
// }

public class MapGenerationSettings
{
    public int XAxis { get; init; }

    public int YAxis { get; init; }

    public int Seed { get; init; }
}

public class MapGenerationService
{
    /// <summary>
    ///     Method <c>StartAsync</c> starts the map generation process asynchronously in Colony.Engine.
    /// </summary>
    public async Task StartAsync(MapGenerationSettings settings, CancellationToken cancellationToken = default)
    {
    }
}