namespace Colony.Engine.Generation.Models;

public sealed record MapGenerationProgress(MapGenerationStep Step)
{
    public bool IsCompleted => Step == MapGenerationStep.Completed;
}