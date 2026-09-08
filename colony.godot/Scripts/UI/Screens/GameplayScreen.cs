using System;
using Colony.Godot.Scripts.Infrastructure.Navigation;
using Godot;

namespace Colony.Godot.Scripts.UI.Screens;

public sealed partial class GameplayScreen : BaseScreen
{
    private readonly IScreenNavigator _navigator;

    public GameplayScreen(IScreenNavigator navigator)
    {
        Console.WriteLine("Creating GameplayScreen");
        _navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
        Build();
    }

    private void Build()
    {
        Console.WriteLine("Building GameplayScreen");
        Name = RouteNames.Gameplay;
        SetAnchorsPreset(LayoutPreset.FullRect);

        var title = new Label
        {
            Text = "Gameplay",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Position = new Vector2(0, 120),
        };
        title.SetAnchorsPreset(LayoutPreset.TopWide);
        AddChild(title);

        var backButton = new Button
        {
            Text = "Back to menu",
            CustomMinimumSize = new Vector2(220, 52),
        };
        backButton.SetAnchorsPreset(LayoutPreset.Center);
        backButton.Pressed += () => _navigator.NavigateBack();
        AddChild(backButton);
    }
}