using System;
using System.Collections.Generic;
using Colony.Engine.World;
using Godot;

namespace Colony.Godot.Scripts.Rendering;

public sealed class WorldRenderer
{
    private Node3D _root = null!;
    private readonly LayerRenderer _layerRenderer;
    
    private readonly Dictionary<int, Node3D> _layers = new();
    private int _selectedLayer;
    private LayerVisibilityMode _layerVisibilityMode;

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
        
        // Define initial values
        _selectedLayer = 0;
        _layerVisibilityMode = LayerVisibilityMode.SelectedAndBelow;

        for (var layer = 0; layer < grid.LayerCount; layer++)
        {
            var layerNode = _layerRenderer.Render(grid, layer);
            
            _layers.Add(layer, layerNode);

            _root.AddChild(layerNode);
        }
        
        ApplyVisibility();

        return _root;
    }

    public void SetSelectedLayer(int layer)
    {
        if (!_layers.ContainsKey(layer))
        {
            throw new ArgumentOutOfRangeException(nameof(layer));
        }
        
        _selectedLayer = layer;

        ApplyVisibility();
    }
    
    public void SetVisibilityMode(LayerVisibilityMode mode)
    {
        _layerVisibilityMode = mode;

        ApplyVisibility();
    }
    
    private void ApplyVisibility()
    {
        foreach (var pair in _layers)
        {
            var layer = pair.Key;
            var node = pair.Value;

            node.Visible = _layerVisibilityMode switch
            {
                LayerVisibilityMode.SelectedAndBelow
                    => layer <= _selectedLayer,

                _ => false
            };
        }
    }
}