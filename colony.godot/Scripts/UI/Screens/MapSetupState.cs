using System;
using Colony.Godot.Scripts.UI.Screens.MapSetup.models;

namespace Colony.Godot.Scripts.UI.Screens;

public class MapSetupState
{
    // Constants
    private static readonly MapSetupState DefaultState = new(
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

    private MapSetupState(MapSetupForm form, bool isGenerating)
    {
        Form = form;
        IsGenerating = isGenerating;
    }

    public MapSetupState() : this(
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