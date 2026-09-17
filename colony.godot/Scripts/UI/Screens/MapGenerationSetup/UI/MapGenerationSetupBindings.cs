using System;
using Colony.Godot.Scripts.UI.Screens.MapGenerationSetup.models;

namespace Colony.Godot.Scripts.UI.Screens.MapGenerationSetup.UI;

public class MapGenerationSetupBindings
{
    public required MapSize InitialMapSize { get; init; }
    public required int InitialSeed { get; init; }

    public required Action Generate { get; init; }
    public required Action Play { get; init; }
    public required Action<MapSize> SelectMapSize { get; init; }
    public required Action<int> UpdateSeed { get; init; }
}