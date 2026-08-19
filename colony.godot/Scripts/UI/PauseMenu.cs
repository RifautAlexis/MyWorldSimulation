using System;
using Godot;

namespace Colony.Godot.Scripts.UI;

public partial class PauseMenu : Control
{
    private Button _mainMenuButton = null!;

    private ColorRect _overlay = null!;
    private PanelContainer _panel = null!;
    private Button _quitButton = null!;

    private Button _resumeButton = null!;
    private Button _settingsButton = null!;

    public event Action? ResumeRequested;
    public event Action? SettingsRequested;
    public event Action? MainMenuRequested;
    public event Action? QuitRequested;

    public override void _Ready()
    {
        SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);

        MouseFilter = MouseFilterEnum.Stop;

        BuildUi();

        Hide();
    }

    private void BuildUi()
    {
        BuildOverlay();
        BuildMenu();
    }

    private void BuildOverlay()
    {
        _overlay = new ColorRect
        {
            Color = new Color(0f, 0f, 0f, 0.55f),
            MouseFilter = MouseFilterEnum.Stop,
        };

        _overlay.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);

        AddChild(_overlay);
    }

    private void BuildMenu()
    {
        var centerContainer = new CenterContainer
        {
            MouseFilter = MouseFilterEnum.Ignore,
        };

        centerContainer.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);

        AddChild(centerContainer);

        _panel = new PanelContainer
        {
            CustomMinimumSize = new Vector2(360f, 0f),
            MouseFilter = MouseFilterEnum.Stop,
        };

        centerContainer.AddChild(_panel);

        var marginContainer = new MarginContainer();

        marginContainer.AddThemeConstantOverride("margin_left", 32);
        marginContainer.AddThemeConstantOverride("margin_top", 32);
        marginContainer.AddThemeConstantOverride("margin_right", 32);
        marginContainer.AddThemeConstantOverride("margin_bottom", 32);

        _panel.AddChild(marginContainer);

        var verticalContainer = new VBoxContainer
        {
            Alignment = BoxContainer.AlignmentMode.Center,
        };

        verticalContainer.AddThemeConstantOverride("separation", 16);

        marginContainer.AddChild(verticalContainer);

        BuildTitle(verticalContainer);
        BuildButtons(verticalContainer);
    }

    private void BuildTitle(VBoxContainer container)
    {
        var title = new Label
        {
            Text = "Paused",
            HorizontalAlignment = HorizontalAlignment.Center,
            CustomMinimumSize = new Vector2(0f, 50f),
        };

        title.AddThemeFontSizeOverride("font_size", 32);

        container.AddChild(title);
    }

    private void BuildButtons(VBoxContainer container)
    {
        _resumeButton = CreateButton(
            "Resume",
            OnResumePressed);

        _settingsButton = CreateButton(
            "Settings",
            OnSettingsPressed);
        _settingsButton.SetDisabled(true);

        _mainMenuButton = CreateButton(
            "Main Menu",
            OnMainMenuPressed);

        _quitButton = CreateButton(
            "Quit",
            OnQuitPressed);

        container.AddChild(_resumeButton);
        container.AddChild(_settingsButton);
        container.AddChild(_mainMenuButton);
        container.AddChild(_quitButton);
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

    private void OnResumePressed()
    {
        ResumeRequested?.Invoke();
    }

    private void OnSettingsPressed()
    {
        SettingsRequested?.Invoke();
    }

    private void OnMainMenuPressed()
    {
        MainMenuRequested?.Invoke();
    }

    private void OnQuitPressed()
    {
        QuitRequested?.Invoke();
    }
}