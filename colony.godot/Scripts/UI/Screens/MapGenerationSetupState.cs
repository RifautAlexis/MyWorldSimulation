using System;
using Colony.Godot.Scripts.UI.Screens.MapSetup.models;

namespace Colony.Godot.Scripts.UI.Screens;

public class MapGenerationSetupState
{
    // Constants
    public static readonly MapGenerationSetupState DefaultState = new(
        new MapSetupForm
        {
            MapSize = MapSize.Medium,
            Seed = 12345,
        },
        false
    );

    // State properties
    public MapSetupForm Form { get; private set; }
    public bool IsGenerating { get; private set; }

    private MapGenerationSetupState(MapSetupForm form, bool isGenerating)
    {
        Form = form;
        IsGenerating = isGenerating;
    }

    public MapGenerationSetupState() : this(
        new MapSetupForm
        {
            MapSize = DefaultState.Form.MapSize,
            Seed = DefaultState.Form.Seed,
        },
        DefaultState.IsGenerating
    )
    {
    }

    public void SetMapSize(MapSize mapSize)
    {
        Form.MapSize = mapSize;
        FormChanged?.Invoke();
    }

    public void SetSeed(int seed)
    {
        Form.Seed = seed;
        FormChanged?.Invoke();
    }

    public void SetForm(MapSetupForm form)
    {
        Form = form;
        FormChanged?.Invoke();
    }

    public void SetIsGenerating(bool isGenerating)
    {
        IsGenerating = isGenerating;
        IsGeneratingChanged?.Invoke();
    }

    // Action handlers
    public event Action? FormChanged;
    public event Action? IsGeneratingChanged;
}