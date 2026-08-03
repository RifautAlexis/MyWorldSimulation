using Colony.Engine.World;
using Godot;

namespace Colony.Godot.Scripts.Rendering;

public sealed class LayerRenderer
{
    public Node3D Render(Grid grid, int layer)
    {
        var root = new Node3D
        {
            Name = $"Layer_{layer}",
        };

        foreach (var cell in grid.GetLayer(layer))
        {
            var node = CreateCell(cell);

            if (node == null)
            {
                continue;
            }

            node.Position = ToWorldPosition(cell.Position);

            root.AddChild(node);
        }

        return root;
    }

    private MeshInstance3D? CreateCell(Cell cell)
    {
        if (cell.TerrainType == TerrainType.Air)
        {
            return null;
        }

        var mesh = new MeshInstance3D
        {
            Mesh = new BoxMesh
            {
                Size = new Vector3(0.95f, 0.1f, 0.95f)
            },
            MaterialOverride = CreateMaterial(cell.TerrainType),
        };

        return mesh;
    }

    private Vector3 ToWorldPosition(CellPosition position)
    {
        return new Vector3(position.X, position.Layer, position.Y);
    }

    private StandardMaterial3D CreateMaterial(TerrainType terrainType)
    {
        return terrainType switch
        {
            TerrainType.Air => new StandardMaterial3D { AlbedoColor = Colors.Transparent },
            TerrainType.Soil => new StandardMaterial3D { AlbedoColor = Colors.Brown },
            TerrainType.Rock => new StandardMaterial3D { AlbedoColor = Colors.Gray },
            TerrainType.Water => new StandardMaterial3D { AlbedoColor = Colors.Blue },
            _ => new StandardMaterial3D { AlbedoColor = Colors.DeepPink },
        };
    }
}