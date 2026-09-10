using System;
using System.Collections.Generic;
using Colony.Godot.Scripts.UI.Screens;
using Godot;

namespace Colony.Godot.Scripts.Infrastructure.Navigation;

public sealed class ScreenNavigator : IScreenNavigator
{
    private readonly Stack<BaseScreen> _history = new();
    private readonly Node _root;
    private readonly Dictionary<string, Func<BaseScreen>> _routes = new(StringComparer.OrdinalIgnoreCase);
    private object? _currentPayload;

    public ScreenNavigator(Node root)
    {
        _root = root ?? throw new ArgumentNullException(nameof(root));
    }

    public bool CanNavigateBack => _history.Count > 1;

    public void Register(string route, Func<BaseScreen> factory)
    {
        if (string.IsNullOrWhiteSpace(route)) throw new ArgumentException("Route cannot be empty.", nameof(route));

        _routes[route] = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public TPayload? GetPayload<TPayload>() where TPayload : class
    {
        return _currentPayload as TPayload;
    }

    public void ClearPayload()
    {
        _currentPayload = null;
    }

    public void NavigateBack()
    {
        if (_history.Count <= 1) return;

        var current = _history.Pop();
        current.OnExit();
        _root.RemoveChild(current);
        current.QueueFree();

        Console.WriteLine(
            $"Navigated back from {current.Name} to {_history.Peek().Name}. History count : {_history.Count}");
        var previous = _history.Peek();
        previous.Show();
        _root.AddChild(previous);
        previous.OnEnter();
    }

    public void NavigateTo(string route, object? payload = null)
    {
        if (!_root.IsInsideTree() || !_root.IsNodeReady())
            throw new InvalidOperationException("Cannot navigate before the navigator root is ready.");

        if (!_routes.TryGetValue(route, out var factory))
            throw new InvalidOperationException($"No screen registered for route '{route}'.");

        ClearPayload();

        _currentPayload = payload;
        var screen = factory();
        if (screen is null) throw new InvalidOperationException($"Factory for route '{route}' returned a null screen.");

        if (_history.Count > 0)
        {
            var current = _history.Peek();
            current.OnExit();
            current.Hide();
            _root.RemoveChild(current);
        }

        _root.AddChild(screen);
        screen.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        _history.Push(screen);
        screen.OnEnter();
    }

    public void Clear()
    {
        while (_history.Count > 0)
        {
            var screen = _history.Pop();

            if (screen == null)
                continue;

            if (screen.IsInsideTree())
                _root.RemoveChild(screen);

            screen.QueueFree();
        }
    }
}