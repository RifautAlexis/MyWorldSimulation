using Colony.Engine.World;
using Godot;

namespace Colony.Godot.Scripts.Rendering;

public sealed class LayerRenderer
{
    private const float CellSize = 1.0f;
    private const float BorderOffset = 0.001f;
    
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

            // node.Position = ToWorldPosition(cell.Position);

            root.AddChild(node);
        }

        var borders = CreateBorders(grid, layer);
        root.AddChild(borders);

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
                Size = new Vector3(CellSize, CellSize, CellSize),
            },
            MaterialOverride = CreateMaterial(cell.TerrainType),
            Position = ToWorldPosition(cell.Position),
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
    
    private MeshInstance3D CreateBorders(Grid grid, int layer)
    {
        var mesh = new ImmediateMesh();

        var material = new StandardMaterial3D
        {
            AlbedoColor = new Color(0, 0, 0, 0.35f),
            ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
            Transparency = BaseMaterial3D.TransparencyEnum.Alpha
        };

        mesh.SurfaceBegin(
            Mesh.PrimitiveType.Lines,
            material
        );

        foreach (var cell in grid.GetLayer(layer))
        {
            if (cell.TerrainType == TerrainType.Air)
                continue;

            AddCubeEdges(mesh, ToWorldPosition(cell.Position));
        }

        mesh.SurfaceEnd();

        return new MeshInstance3D
        {
            Name = $"Layer_{layer}_Borders",
            Mesh = mesh
        };
    }

    private void AddCubeEdges(ImmediateMesh mesh, Vector3 center)
    {
        var half = (CellSize / 2.0f) + BorderOffset;

        var min = center - new Vector3(half, half, half);
        var max = center + new Vector3(half, half, half);

        // Bottom
        AddLine(mesh,
            new Vector3(min.X, min.Y, min.Z),
            new Vector3(max.X, min.Y, min.Z));

        AddLine(mesh,
            new Vector3(max.X, min.Y, min.Z),
            new Vector3(max.X, min.Y, max.Z));

        AddLine(mesh,
            new Vector3(max.X, min.Y, max.Z),
            new Vector3(min.X, min.Y, max.Z));

        AddLine(mesh,
            new Vector3(min.X, min.Y, max.Z),
            new Vector3(min.X, min.Y, min.Z));

        // Top
        AddLine(mesh,
            new Vector3(min.X, max.Y, min.Z),
            new Vector3(max.X, max.Y, min.Z));

        AddLine(mesh,
            new Vector3(max.X, max.Y, min.Z),
            new Vector3(max.X, max.Y, max.Z));

        AddLine(mesh,
            new Vector3(max.X, max.Y, max.Z),
            new Vector3(min.X, max.Y, max.Z));

        AddLine(mesh,
            new Vector3(min.X, max.Y, max.Z),
            new Vector3(min.X, max.Y, min.Z));

        // Vertical
        AddLine(mesh,
            new Vector3(min.X, min.Y, min.Z),
            new Vector3(min.X, max.Y, min.Z));

        AddLine(mesh,
            new Vector3(max.X, min.Y, min.Z),
            new Vector3(max.X, max.Y, min.Z));

        AddLine(mesh,
            new Vector3(max.X, min.Y, max.Z),
            new Vector3(max.X, max.Y, max.Z));

        AddLine(mesh,
            new Vector3(min.X, min.Y, max.Z),
            new Vector3(min.X, max.Y, max.Z));
    }

    private void AddLine(
        ImmediateMesh mesh,
        Vector3 start,
        Vector3 end)
    {
        mesh.SurfaceAddVertex(start);
        mesh.SurfaceAddVertex(end);
    }
}