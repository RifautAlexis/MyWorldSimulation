using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Colony.Godot.Scripts.Events;

public class EventBus : IEventBus
{
    private readonly ConcurrentDictionary<Type, List<Delegate>> _handlers = new();

    public void Publish<T>(T @event)
    {
        if (!_handlers.TryGetValue(typeof(T), out var handlers))
            return;

        foreach (var handler in handlers.ToList())
        {

            if (handler is Action<T> action)
            {
                action(@event);
            }
            else
            {
                throw new InvalidOperationException(
                    "Invalid event handler registration");
            }
        }
    }

    public void Subscribe<T>(Action<T> handler)
    {
        var handlers = _handlers.GetOrAdd(typeof(T), _ => new List<Delegate>());
        handlers.Add(handler);
    }

    public void Unsubscribe<T>(Action<T> handler)
    {
        if (!_handlers.TryGetValue(typeof(T), out var handlers))
            return;

        handlers.Remove(handler);
    }
}