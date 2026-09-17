using System;
using Colony.Godot.Scripts.UI.Screens.MapSetup.models;

namespace Colony.Godot.Scripts.UI.Screens.MapSetup.UI;

public class MapSetupBindings
{
    public required Action Generate { get; init; }
    public required Action Play { get; init; }
    public required Action<MapSize> SelectMapSize { get; init; }
    public required Action<int> UpdateSeed { get; init; }
}