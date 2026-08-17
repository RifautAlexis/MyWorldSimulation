using Colony.Engine.Generation;

namespace Colony.Engine.World;

public sealed class TerrainGenerator
{
    private readonly WorldConfiguration _configuration;
    private readonly int _minimumSurfaceLayer;
    private readonly int _maximumSurfaceLayer;

    public TerrainGenerator(WorldConfiguration configuration)
    {
        _configuration = configuration;

        if (_configuration.Width <= 0)
            throw new ArgumentOutOfRangeException(nameof(configuration), "World width must be positive.");

        if (_configuration.Height <= 0)
            throw new ArgumentOutOfRangeException(nameof(configuration), "World height must be positive.");

        if (_configuration.LayerCount < 3)
            throw new ArgumentOutOfRangeException(nameof(configuration), "World must contain at least three layers.");

        _maximumSurfaceLayer = Math.Clamp(_configuration.MaximumSurfaceLayer, 1, _configuration.LayerCount - 2);
        _minimumSurfaceLayer = Math.Clamp(_configuration.MinimumSurfaceLayer, 1, _maximumSurfaceLayer);
    }

    public TerrainType Generate(CellPosition position)
    {
        var surfaceLayer = GetSurfaceLayer(position.X, position.Y);

        if (position.Layer > surfaceLayer)
            return TerrainType.Air;

        if (ShouldCarveCave(position, surfaceLayer))
            return TerrainType.Air;

        var soilDepth = GetSoilDepth(position.X, position.Y);
        var depthBelowSurface = surfaceLayer - position.Layer;

        return depthBelowSurface <= soilDepth
            ? TerrainType.Soil
            : TerrainType.Rock;
    }

    private int GetSurfaceLayer(int x, int y)
    {
        var baseElevation = Normalize(ProceduralNoise.Fractal2D(x, y,
                                                                _configuration.SurfaceFrequency,
                                                                _configuration.SurfaceOctaves,
                                                                _configuration.Seed));
        var ridgeElevation = 1d - Math.Abs(ProceduralNoise.Fractal2D(x, y,
                                                                     _configuration.SurfaceFrequency * 1.9d,
                                                                     Math.Max(2, _configuration.SurfaceOctaves - 1),
                                                                     _configuration.Seed + 101));
        var detailElevation = Normalize(ProceduralNoise.Fractal2D(x, y,
                                                                  _configuration.SurfaceFrequency * 3.7d,
                                                                  2,
                                                                  _configuration.Seed + 211));
        var combinedElevation = (baseElevation * 0.55d) +
                                (ridgeElevation * 0.30d) +
                                (detailElevation * 0.15d);
        var heightRange = _maximumSurfaceLayer - _minimumSurfaceLayer;

        return _minimumSurfaceLayer + (int)Math.Round(combinedElevation * heightRange);
    }

    private int GetSoilDepth(int x, int y)
    {
        if (_configuration.MinimumSoilDepth >= _configuration.MaximumSoilDepth)
            return _configuration.MinimumSoilDepth;

        var soilDepthNoise = Normalize(ProceduralNoise.Fractal2D(x, y,
                                                                 _configuration.SurfaceFrequency * 2.5d,
                                                                 2,
                                                                 _configuration.Seed + 307));
        var soilDepthRange = _configuration.MaximumSoilDepth - _configuration.MinimumSoilDepth;

        return _configuration.MinimumSoilDepth + (int)Math.Round(soilDepthNoise * soilDepthRange);
    }

    private bool ShouldCarveCave(CellPosition position, int surfaceLayer)
    {
        if (position.Layer <= 0)
            return false;

        var depthBelowSurface = surfaceLayer - position.Layer;

        if (depthBelowSurface <= _configuration.CaveSurfaceBuffer)
            return false;

        var caveShape = 1d - Math.Abs(ProceduralNoise.Fractal3D(position.X,
                                                                position.Y,
                                                                position.Layer,
                                                                _configuration.CaveFrequency,
                                                                3,
                                                                _configuration.Seed + 401));
        var depthFactor = Math.Min(1d, depthBelowSurface / (double)Math.Max(1, surfaceLayer));
        var adaptiveThreshold = _configuration.CaveThreshold - (depthFactor * 0.12d);

        return caveShape >= adaptiveThreshold;
    }

    private static double Normalize(double value)
    {
        return (value + 1d) * 0.5d;
    }
}