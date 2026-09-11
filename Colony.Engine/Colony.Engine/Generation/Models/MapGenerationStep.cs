namespace Colony.Engine.Generation.Models;

public enum MapGenerationStep
{
    Initializing,
    GeneratingMap,
    GeneratingTerrain,
    GeneratingResources,
    GeneratingWater,
    Completed,
}

// map : representation of the world, including terrain, water, and resources, generated based on the configuration and procedural noise.
// Voxel/Cell : 3D representation of a block in the world, with properties like type, density, and material.