using System;
using Colony.Godot.Scripts.Events;
using Colony.Godot.Scripts.Infrastructure.DependencyInjection;
using Godot;

namespace Colony.Godot.Scripts.Screens;

public partial class MainMenu : Control, IInject<IEventBus>
{
    private IEventBus _eventBus = null!;
    
    private Button _playButton;
    private Button _exitButton;

    public void Inject(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    
    public override void _Ready()
    {
        BuildUI();
        ConnectEvents();
    }

    private void BuildUI()
    {
        var center = new CenterContainer();

        center.SetAnchorsAndOffsetsPreset(
            LayoutPreset.FullRect
        );
        AddChild(center);


        var layout = new VBoxContainer();
        center.AddChild(layout);


        _playButton = new Button
        {
            Text = "Play"
        };
        _exitButton = new Button
        {
            Text = "Exit"
        };

        layout.AddChild(_playButton);
        layout.AddChild(_exitButton);
    }


    private void ConnectEvents()
    {
        _playButton.Pressed += OnPlayPressed;
        _exitButton.Pressed += OnExitPressed;
    }


    private void OnPlayPressed()
    {
        _eventBus.Publish(new NewGameRequested());
    }


    private void OnExitPressed()
    {
        GetTree().Quit();
    }
}