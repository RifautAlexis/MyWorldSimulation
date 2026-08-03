using System;
using System.Collections.Generic;
using Colony.Engine.World;
using Godot;

namespace Colony.Godot.Scripts.Rendering;

public sealed class WorldRenderer
{
    private readonly LayerRenderer _layerRenderer;
    private Node3D _root = null!;
    private readonly Dictionary<int, Node3D> _layers = new();

    public WorldRenderer(LayerRenderer layerRenderer)
    {
        _layerRenderer = layerRenderer;
    }

    public Node3D Build(Grid grid)
    {
        _root = new Node3D
        {
            Name = "World",
        };

        _layers.Clear();

        for (var layer = 0; layer < grid.LayerCount; layer++)
        {
            var layerNode = _layerRenderer.Render(grid, layer);
            
            _layers.Add(layer, layerNode);

            _root.AddChild(layerNode);
        }

        return _root;
    }

    public void SetLayerVisible(int layer, bool visible)
    {
        if (!_layers.TryGetValue(layer, out var layerNode))
        {
            throw new ArgumentOutOfRangeException(nameof(layer));
        }
        
        layerNode.Visible = visible;
    }
    public void ShowOnlyLayer(int layer)
    {
        foreach (var pair in _layers)
        {
            pair.Value.Visible = pair.Key == layer;
        }
    }
}