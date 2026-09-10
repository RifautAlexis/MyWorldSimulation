using System;
using Colony.Godot.Scripts.Infrastructure.Navigation;
using Godot;

namespace Colony.Godot.Scripts.UI.Screens;

public sealed partial class MainMenuScreen : BaseScreen
{
    public MainMenuScreen(IScreenNavigator navigator) : base(navigator)
    {
        Build();
    }

    private void Build()
    {
        Console.WriteLine("Building MainMenuScreen");
        Name = RouteNames.MainMenu;
        SetAnchorsPreset(LayoutPreset.FullRect);

        var title = new Label
        {
            Text = "Colony",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Position = new Vector2(0, 120),
        };
        title.SetAnchorsPreset(LayoutPreset.TopWide);
        AddChild(title);

        var layout = new VBoxContainer
        {
            CustomMinimumSize = new Vector2(220, 52),
        };
        layout.SetAnchorsPreset(LayoutPreset.Center);
        AddChild(layout);

        var startButton = new Button
        {
            Text = "Start game",
            CustomMinimumSize = new Vector2(220, 52),
        };
        startButton.SetAnchorsPreset(LayoutPreset.Center);
        startButton.Pressed += () => _navigator.NavigateTo(RouteNames.MapGenerationSetup);
        layout.AddChild(startButton);

        var exitButton = new Button
        {
            Text = "Exit",
            CustomMinimumSize = new Vector2(220, 52),
        };
        exitButton.SetAnchorsPreset(LayoutPreset.Center);
        exitButton.Pressed += () => GetTree().Quit();
        layout.AddChild(exitButton);
    }
}