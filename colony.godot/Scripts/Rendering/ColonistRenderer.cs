using System.Collections.Generic;
using Colony.Engine.Simulation.Views;
using Godot;

namespace Colony.Godot.Scripts.Rendering;

public sealed class ColonistRenderer
{
    private readonly Dictionary<int, Node3D> _nodes = [];

    public Node3D Build(ColonistView colonist)
    {
        var node = new MeshInstance3D
        {
            Name = $"Colonist_{colonist.Id}",
            Mesh = new SphereMesh
            {
                Radius = 0.3f,
                Height = 0.6f,
            },
        };

        _nodes[colonist.Id] = node;

        Update(node, colonist);

        return node;
    }

    public void Update(ColonistView colonist)
    {
        if (!_nodes.TryGetValue(colonist.Id, out var node))
            return;

        Update(node, colonist);
    }

    private static void Update(Node3D node, ColonistView colonist)
    {
        node.Position = ToWorldPosition(colonist);
    }

    private static Vector3 ToWorldPosition(ColonistView colonist)
    {
        return new Vector3(
            colonist.X,
            colonist.Layer + 1f,
            colonist.Z);
    }
}