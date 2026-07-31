# Colony.Godot

# DI
Composed in two worlds :
```mermaid
            Godot
                │
                ▼
           Application
                │
    ┌──────────┴──────────┐
    │                     │
    ▼                     ▼
Scene Tree          Service Provider
(Godot)                  (DI)
    │                     │
    ▼                     ▼
MainMenu, World3D     Engine, Services,
Camera, Buttons       EventBus, Clock...
```

## Rule #1 — The scene tree owns visual objects
These should be created with `new` and added to the scene tree.

Examples:
```c#
new Button();

new Camera3D();

new MeshInstance3D();

new Node3D();

new Label();
```
They belong to Godot.

## Rule #2 — DI owns application services
Examples:

```c#
SimulationEngine

GameClock

EventBus

GridRenderer

CameraController

WorldGenerator

SaveGameService
```

These don't exist because Godot needs them.

They exist because **your application** needs them.

### Wait... GridRenderer?

You may be thinking:
$$
"But GridRenderer draws things!"
$$

Exactly!

But notice the difference.

A `GridRenderer` is not a node.

Instead:
```mermaid
GridRenderer
       │
       ▼
creates MeshInstance3D
```

The renderer is just a C# service.

The meshes it creates belong to Godot.

## Rule #3 — Never call `GetNode()` inside services

For example:
```c#
public class GridRenderer
{
    public void Render(...)
    {
        // ❌ Don't do this
        GetNode<MeshInstance3D>("...");
    }
}
```

A service shouldn't know where it lives in the scene tree.

Instead:
```c#
public class GridRenderer
{
    public void Render(Node3D parent)
    {
        ...
    }
}
```

or even better:
```c#
public class GridRenderer
{
    public Node3D BuildGrid(...)
    {
        ...
    }
}
```

The service creates visual nodes, but doesn't own them.