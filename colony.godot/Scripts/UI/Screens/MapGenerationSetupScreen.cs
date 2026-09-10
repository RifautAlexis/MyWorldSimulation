using System;
using Colony.Godot.Scripts.Infrastructure.Navigation;
using Colony.Godot.Scripts.UI.Components;
using Godot;

namespace Colony.Godot.Scripts.UI.Screens.MapGenerationSetupScreen;

public class MapGenerationSettings
{
    public int XAxis { get; init; }

    public int YAxis { get; init; }

    public int Seed { get; init; }
}

public class MapSize
{
    public static readonly MapSize Small = new() { XAxis = 32, YAxis = 32 };
    public static readonly MapSize Medium = new() { XAxis = 64, YAxis = 64 };
    public static readonly MapSize Large = new() { XAxis = 128, YAxis = 128 };
    public int XAxis { get; set; }
    public int YAxis { get; set; }
}

public class MapGenerationSetupForm
{
    public MapSize MapSize { get; set; } = new()
    {
        XAxis = 64,
        YAxis = 64,
    };

    public int Seed { get; set; } = 12345;
}

public partial class MapGenerationSetupScreen(IScreenNavigator navigator) : BaseScreen(navigator)
{
    private MapGenerationSetupForm _form = null!;

    private Button _generateButton = null!;
    private Button _largeSizeMapButton = null!;
    private Button _mediumSizeMapButton = null!;
    private Button _smallSizeMapButton = null!;

    public override void _Ready()
    {
        _form = InitializeForm();

        BuildUi();
    }

    #region Form Logic

    private MapGenerationSetupForm InitializeForm()
    {
        return new MapGenerationSetupForm
        {
            MapSize = MapSize.Small,
            Seed = 12345,
        };
    }

    private bool IsFormValid()
    {
        return _form.MapSize is { XAxis: > 0, YAxis: > 0 } && _form.Seed != null;
    }

    private void SelectMapSize(int xAxis, int yAxis)
    {
        _form.MapSize = new MapSize
        {
            XAxis = xAxis,
            YAxis = yAxis,
        };

        UpdateGenerateButton();
    }

    private void UpdateGenerateButton()
    {
        _generateButton.Disabled = !IsFormValid();
    }

    private void OnGeneratePressed()
    {
        Console.WriteLine(
            $"Generate button pressed with MapSize: {_form.MapSize.XAxis}x{_form.MapSize.YAxis}, Seed: {_form.Seed}");

        if (!IsFormValid()) return;

        var mapGenerationSettings = new MapGenerationSettings
        {
            XAxis = _form.MapSize.XAxis,
            YAxis = _form.MapSize.YAxis,
            Seed = _form.Seed,
        };

        _navigator.NavigateTo(RouteNames.Loading, mapGenerationSettings);

        // _mapGenerationService.StartAsync(new MapGenerationSettings
        // {
        //     XAxis = _form.MapSize.XAxis,
        //     YAxis = _form.MapSize.YAxis,
        //     Seed = _form.Seed,
        // });
        // _eventBus.Publish(new NewGameRequested
        // {
        //     SetupForm = _form,
        // });
    }

    #endregion

    #region UI Building Methods

    private void BuildUi()
    {
        var centerContainer = new CenterContainer
        {
            MouseFilter = MouseFilterEnum.Ignore,
        };

        centerContainer.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);

        AddChild(centerContainer);

        var mainColumn = new VBoxContainer();
        centerContainer.AddChild(mainColumn);

        var firstRow = BuildRow();
        var secondRow = BuildRow();
        mainColumn.AddChild(firstRow);
        mainColumn.AddChild(secondRow);

        var sizeSettingsColumn = BuildSizeSettingsColumn();
        var customSettingsColumn = BuildCustomSettingsColumn();

        firstRow.AddChild(sizeSettingsColumn);
        firstRow.AddChild(customSettingsColumn);

        _generateButton = CreateButton(
            "Generate",
            OnGeneratePressed);
        secondRow.AddChild(_generateButton);
    }

    private HBoxContainer BuildRow()
    {
        var row = new HBoxContainer
        {
            MouseFilter = MouseFilterEnum.Stop,
        };

        row.SetAnchorsAndOffsetsPreset(LayoutPreset.Center);

        return row;
    }

    private VBoxContainer BuildSizeSettingsColumn()
    {
        var sizeSettingsColumn = new VBoxContainer();

        var titleLabel = BuildTitle("Map Size");
        sizeSettingsColumn.AddChild(titleLabel);

        _largeSizeMapButton = CreateButton(
            "Large",
            () => SelectMapSize(MapSize.Large.XAxis, MapSize.Large.YAxis));
        _mediumSizeMapButton = CreateButton(
            "Medium",
            () => SelectMapSize(MapSize.Medium.XAxis, MapSize.Medium.YAxis));
        _smallSizeMapButton = CreateButton(
            "Small",
            () => SelectMapSize(MapSize.Small.XAxis, MapSize.Small.YAxis));

        sizeSettingsColumn.AddChild(_largeSizeMapButton);
        sizeSettingsColumn.AddChild(_mediumSizeMapButton);
        sizeSettingsColumn.AddChild(_smallSizeMapButton);

        return sizeSettingsColumn;
    }

    private VBoxContainer BuildCustomSettingsColumn()
    {
        var CustomSettingsColumn = new VBoxContainer();
        var titleLabel = BuildTitle("Custom Settings");
        CustomSettingsColumn.AddChild(titleLabel);

        var seedInput = new InputNumber
        {
            MinValue = int.MinValue,
            MaxValue = int.MaxValue,
            AllowNegative = true,
            Value = _form.Seed,
        };
        seedInput.ValueChanged += value =>
        {
            if (value == null) return;

            _form.Seed = (int)value;
            Console.WriteLine(value);
        };
        CustomSettingsColumn.AddChild(seedInput);

        return CustomSettingsColumn;
    }

    private Label BuildTitle(string titleText)
    {
        var titleLabel = new Label
        {
            Text = titleText,
            HorizontalAlignment = HorizontalAlignment.Center,
            CustomMinimumSize = new Vector2(0f, 50f),
        };

        titleLabel.AddThemeFontSizeOverride("font_size", 32);

        return titleLabel;
    }

    private static Button CreateButton(string text, Action callback)
    {
        var button = new Button
        {
            Text = text,
            CustomMinimumSize = new Vector2(0f, 50f),
            FocusMode = FocusModeEnum.All,
        };

        button.Pressed += callback;

        return button;
    }

    #endregion
}