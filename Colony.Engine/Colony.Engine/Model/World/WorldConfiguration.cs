namespace Colony.Engine.World;

public sealed class WorldConfiguration
{
    public int Width { get; init; }

    public int Height { get; init; }

    public int LayerCount { get; init; }

    public int Seed { get; init; } = 12345;

    public int MinimumSurfaceLayer { get; init; } = 16;

    public int MaximumSurfaceLayer { get; init; } = 30;

    public int MinimumSoilDepth { get; init; } = 2;

    public int MaximumSoilDepth { get; init; } = 5;

    public double SurfaceFrequency { get; init; } = 0.045d;

    public int SurfaceOctaves { get; init; } = 4;

    public double CaveFrequency { get; init; } = 0.09d;

    public double CaveThreshold { get; init; } = 0.72d;

    public int CaveSurfaceBuffer { get; init; } = 3;
}