using System;
using Colony.Engine.Domain;
using Colony.Engine.Facade.Contracts;

namespace Colony.Engine.Facade;

public class ColonyEngineFacade : IColonyEngine
{
    public Map GenerateMap(MapGenerationSettings settings)
    {
        throw new NotImplementedException();
    }
}