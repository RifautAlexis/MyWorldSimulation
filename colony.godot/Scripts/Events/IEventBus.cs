using System;

namespace Colony.Godot.Scripts.Events;

public interface IEventBus
{
    void Publish<T>(T @event);
    void Subscribe<T>(Action<T> handler);
    void Unsubscribe<T>(Action<T> handler);
}