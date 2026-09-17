using System;
using Colony.Godot.Scripts.UI.Components;
using Colony.Godot.Scripts.UI.Screens.MapGenerationSetup.models;
using Godot;

namespace Colony.Godot.Scripts.UI.Screens.MapGenerationSetup.UI;

public class MapSetupForm
{
    // Root Control
    private Control HudRoot { get; set; }

    // UI Elements
    public Button GenerateButton { get; private set; } = null!;
    public Button PlayButton { get; private set; } = null!;

    public Control GetHudRoot()
    {
        return HudRoot;
    }

    public void Show()
    {
        HudRoot.Visible = true;
    }

    public void Hide()
    {
        HudRoot.Visible = false;
    }

    public void SetGenerateButtonEnabled(bool enabled)
    {
        GenerateButton.Disabled = !enabled;
    }

    public void SetPlayButtonEnabled(bool enabled)
    {
        PlayButton.Disabled = !enabled;
    }

    public Control BuildMapSetupForm(MapGenerationSetupBindings bindings)
    {
        HudRoot = new Control
        {
            Name = "HudRoot",
        };
        HudRoot.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);

        // Content wrapper
        var panelContainer = new PanelContainer();
        var marginContainer = new MarginContainer();

        marginContainer.AddThemeConstantOverride("margin_top", 10);
        marginContainer.AddThemeConstantOverride("margin_left", 10);
        marginContainer.AddThemeConstantOverride("margin_bottom", 10);
        marginContainer.AddThemeConstantOverride("margin_right", 10);

        panelContainer.AddChild(marginContainer);

        var vBoxContainer = new VBoxContainer();
        marginContainer.AddChild(vBoxContainer);

        HudRoot.AddChild(panelContainer);

        // Map Size Section
        vBoxContainer.AddChild(MapSizeSection(bindings));

        // Seed Section
        vBoxContainer.AddChild(SeedSection(bindings));

        // Actions Section
        vBoxContainer.AddChild(ActionsSection(bindings));

        return HudRoot;
    }

    private VBoxContainer MapSizeSection(MapGenerationSetupBindings bindings)
    {
        var vBoxContainer = new VBoxContainer();

        var titleLabel = new Label
        {
            Text = "Map Size",
        };
        var hBoxContainer = new HBoxContainer();

        vBoxContainer.AddChild(titleLabel);
        vBoxContainer.AddChild(hBoxContainer);

        var largeSizeMapButton = CreateButton(
            "Large",
            () => bindings.SelectMapSize(MapSize.Large));
        var mediumSizeMapButton = CreateButton(
            "Medium",
            () => bindings.SelectMapSize(MapSize.Medium));
        var smallSizeMapButton = CreateButton(
            "Small",
            () => bindings.SelectMapSize(MapSize.Small));

        hBoxContainer.AddChild(largeSizeMapButton);
        hBoxContainer.AddChild(mediumSizeMapButton);
        hBoxContainer.AddChild(smallSizeMapButton);

        return vBoxContainer;
    }

    private VBoxContainer SeedSection(MapGenerationSetupBindings bindings)
    {
        var vBoxContainer = new VBoxContainer();

        var titleLabel = new Label
        {
            Text = "Seed",
        };
        vBoxContainer.AddChild(titleLabel);

        var seedInput = new InputNumber
        {
            MinValue = int.MinValue,
            MaxValue = int.MaxValue,
            AllowNegative = true,
            Value = bindings.InitialSeed,
        };
        seedInput.ValueChanged += value => bindings.UpdateSeed((int)value);
        vBoxContainer.AddChild(seedInput);

        return vBoxContainer;
    }

    private VBoxContainer ActionsSection(MapGenerationSetupBindings bindings)
    {
        var vBoxContainer = new VBoxContainer();

        GenerateButton = CreateButton("Generate", bindings.Generate);
        vBoxContainer.AddChild(GenerateButton);

        PlayButton = CreateButton("Play", bindings.Play);
        vBoxContainer.AddChild(PlayButton);

        return vBoxContainer;
    }

    private static Button CreateButton(string text, Action callback)
    {
        var button = new CustomButton
        {
            Text = text,
        };

        button.Pressed += callback;

        return button;
    }
}