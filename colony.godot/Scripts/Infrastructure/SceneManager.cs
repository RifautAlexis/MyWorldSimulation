using System;
using Godot;

namespace Colony.Godot.Scripts.Infrastructure;

public sealed class SceneManager
{
    private Control? _host;
    private Node _currentScreen;

    public void Initialize(Control host)
    {
        _host = host;
    }

    public void Show(Node screen)
    {
        if (_host == null)
            throw new InvalidOperationException(
                "SceneManager has not been initialized."
            );

        _currentScreen?.QueueFree();

        _currentScreen = screen;

        _host.AddChild(_currentScreen);

        if (screen is Control control)
        {
            control.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        }
    }
}