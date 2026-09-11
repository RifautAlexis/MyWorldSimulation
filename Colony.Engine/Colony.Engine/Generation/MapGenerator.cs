using Colony.Engine.Generation.Models;
using Colony.Engine.Model.Map;
using Colony.Engine.World;

namespace Colony.Engine.Generation;

public sealed class MapGenerator
{
    public async Task<Map> GenerateAsync(MapGenerationSettings settings,
                                         IProgress<MapGenerationProgress>? progress = null,
                                         CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(settings);

        // Initialize the map generator.
        ReportStep(
            progress,
            MapGenerationStep.Initializing);

        var context = Initialize(
            settings,
            cancellationToken);

        // Generate the map.
        ReportStep(
            progress,
            MapGenerationStep.GeneratingMap);

        await GenerateMapAsync(
            context,
            cancellationToken);

        // Generate the terrain.
        ReportStep(
            progress,
            MapGenerationStep.GeneratingTerrain);

        await GenerateTerrainAsync(
            context,
            cancellationToken);

        // Generate the resources.
        ReportStep(
            progress,
            MapGenerationStep.GeneratingResources);

        await GenerateTerrainAsync(
            context,
            cancellationToken);

        // Generate the water.
        ReportStep(
            progress,
            MapGenerationStep.GeneratingWater);

        await GenerateWaterAsync(
            context,
            cancellationToken);

        // Finalize the world.
        ReportStep(
            progress,
            MapGenerationStep.Completed);

        var world = FinalizeMap(
            context,
            cancellationToken);

        return world;
    }

    private static void ReportStep(IProgress<MapGenerationProgress> progress,
                                   MapGenerationStep step)
    {
        progress.Report(new MapGenerationProgress(step));
    }

    private static MapGenerationContext Initialize(MapGenerationSettings settings,
                                                   CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var mapSettings = new MapSettings
        {
            Width = settings.Width,
            Height = settings.Height,
            Seed = settings.Seed,
        };
        var map = new Map(mapSettings);

        return new MapGenerationContext
        {
            GenerationSettings = settings,
            Map = map,
        };
    }

    private static async Task GenerateMapAsync(MapGenerationContext context,
                                               CancellationToken cancellationToken)
    {
        var width = context.GenerationSettings.Width;
        var height = context.GenerationSettings.Height;
        var layerCount = context.GenerationSettings.LayerCount;

        for (var layer = 0; layer < layerCount; layer++) await GenerateLayersAsync(context, layer, cancellationToken);

        await Task.Yield();
    }

    private static async Task GenerateLayersAsync(MapGenerationContext context,
                                                  int layer,
                                                  CancellationToken cancellationToken)
    {
        var width = context.GenerationSettings.Width;
        var height = context.GenerationSettings.Height;

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var position = new CellPosition(x, y, layer);
                context.Map.SetCell(position, new Cell(position));
            }

            await Task.Yield();
        }
    }

    private static async Task GenerateTerrainAsync(MapGenerationContext context,
                                                   CancellationToken cancellationToken)
    {
        var random = new Random();
        var timeDelay = random.Next(3, 10); // Random delay between 3 and 10 seconds

        await Task.Delay(timeDelay * 1000, cancellationToken); // time in seconds
    }

    private static async Task GenerateWaterAsync(MapGenerationContext context,
                                                 CancellationToken cancellationToken)
    {
        var random = new Random();
        var timeDelay = random.Next(3, 10); // Random delay between 3 and 10 seconds

        await Task.Delay(timeDelay * 1000, cancellationToken); // time in seconds
    }

    private static async Task GenerateResourcesAsync(MapGenerationContext context,
                                                     CancellationToken cancellationToken)
    {
        var random = new Random();
        var timeDelay = random.Next(3, 10); // Random delay between 3 and 10 seconds

        await Task.Delay(timeDelay * 1000, cancellationToken); // time in seconds
    }

    private static Map FinalizeMap(MapGenerationContext context,
                                   CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Final validation / cleanup / construction.

        return context.Map;
    }
}