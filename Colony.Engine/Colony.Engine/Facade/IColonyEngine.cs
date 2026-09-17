using Colony.Engine.Domain;
using Colony.Engine.Facade.Contracts;

namespace Colony.Engine.Facade;

public interface IColonyEngine
{
    Map GenerateMap(MapGenerationSettings settings);
    // void Update(TimeSpan delta);
    // SaveData Save();
    // void Load(SaveData save);
}