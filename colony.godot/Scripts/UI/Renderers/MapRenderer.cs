using System;
using System.Collections.Generic;
using System.Linq;
using Colony.Engine.Domain;
using Colony.Godot.Scripts.UI.Models;
using Godot;

namespace Colony.Godot.Scripts.UI.Renderers;

public class MapRenderer
{
    private readonly LayerRenderer _layerRenderer;

    private readonly Dictionary<int, Node3D> _layers = new();
    private LayerVisibilityMode _layerVisibilityMode;
    private Node3D _root = null!;
    private int _selectedLayer;

    public MapRenderer(LayerRenderer layerRenderer)
    {
        _layerRenderer = layerRenderer;
    }

    public Node3D Build(Map map)
    {
        _root = new Node3D
        {
            Name = "WorldScreen",
        };

        _layers.Clear();

        // Define initial values
        _selectedLayer = 0;
        _layerVisibilityMode = LayerVisibilityMode.SelectedAndBelow;

        for (var layer = 0; layer < map.Settings.LayerCount; layer++)
        {
            var layerNode = _layerRenderer.Render(map, layer);

            _layers.Add(layer, layerNode);

            _root.AddChild(layerNode);
        }

        foreach (var layer in _layers.Count > 0 ? _layers.Keys : Enumerable.Empty<int>())
        {
            var layerItems = _layers
                            .Where(x => x.Key == layer)
                            .GroupBy(x => x.Key)
                            .Select(g => g.Count());
        }

        ApplyVisibility();

        return _root;
    }

    public void SetSelectedLayer(int layer)
    {
        if (!_layers.ContainsKey(layer)) throw new ArgumentOutOfRangeException(nameof(layer));

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

            // node.Visible = _layerVisibilityMode switch
            // {
            //     LayerVisibilityMode.SelectedAndBelow
            //         => layer <= _selectedLayer,
            //
            //     _ => false,
            // };
            node.Visible = true;
        }
    }
}