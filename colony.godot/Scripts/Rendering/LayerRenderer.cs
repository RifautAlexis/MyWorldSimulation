using System;
using System.Collections.Generic;
using Colony.Engine.World;
using Godot;

namespace Colony.Godot.Scripts.Rendering;

public sealed class LayerRenderer
{
    private const float CellSize = 1.0f;
    private const float BorderOffset = 0.001f;
    private static readonly Vector3 CellDimensions = new(CellSize, CellSize, CellSize);

    private readonly StandardMaterial3D _borderMaterial = new()
    {
        AlbedoColor = new Color(0, 0, 0, 0.35f),
        ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
        Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
    };

    private readonly BoxMesh _cellMesh = new()
    {
        Size = CellDimensions,
    };

    private readonly Dictionary<TerrainType, StandardMaterial3D> _materials = new()
    {
        [TerrainType.Air] = new StandardMaterial3D { AlbedoColor = Colors.Transparent },
        [TerrainType.Soil] = new StandardMaterial3D { AlbedoColor = Colors.Peru },
        [TerrainType.Rock] = new StandardMaterial3D { AlbedoColor = Colors.Gray },
        [TerrainType.Water] = new StandardMaterial3D { AlbedoColor = Colors.Blue },
    };

    public Node3D Render(Grid grid, int layer)
    {
        var root = new Node3D
        {
            Name = $"Layer_{layer}",
        };

        Console.WriteLine($"Rendering layer {layer}...");

        var positionsByTerrain = new Dictionary<TerrainType, List<Vector3>>();
        var solidCellCenters = new List<Vector3>();

        foreach (var cell in grid.GetLayer(layer))
        {
            if (cell.TerrainType == TerrainType.Air)
                continue;

            var position = ToWorldPosition(cell.Position);

            solidCellCenters.Add(position);

            if (!positionsByTerrain.TryGetValue(cell.TerrainType, out var positions))
            {
                positions = new List<Vector3>();
                positionsByTerrain[cell.TerrainType] = positions;
            }

            positions.Add(position);
        }

        foreach (var pair in positionsByTerrain)
            root.AddChild(CreateCells(layer, pair.Key, pair.Value));

        var borders = CreateBorders(layer, solidCellCenters);

        if (borders != null)
            root.AddChild(borders);

        return root;
    }

    private MultiMeshInstance3D CreateCells(int layer,
                                            TerrainType terrainType,
                                            IReadOnlyList<Vector3> positions)
    {
        var multiMesh = new MultiMesh
        {
            TransformFormat = MultiMesh.TransformFormatEnum.Transform3D,
            Mesh = _cellMesh,
            InstanceCount = positions.Count,
        };

        for (var index = 0; index < positions.Count; index++)
            multiMesh.SetInstanceTransform(index, new Transform3D(Basis.Identity, positions[index]));

        return new MultiMeshInstance3D
        {
            Name = $"Layer_{layer}_{terrainType}",
            Multimesh = multiMesh,
            MaterialOverride = GetMaterial(terrainType),
        };
    }

    private Vector3 ToWorldPosition(CellPosition position)
    {
        return new Vector3(position.X, position.Layer, position.Y);
    }

    private StandardMaterial3D GetMaterial(TerrainType terrainType)
    {
        if (_materials.TryGetValue(terrainType, out var material))
            return material;

        var fallback = new StandardMaterial3D { AlbedoColor = Colors.DeepPink };

        _materials[terrainType] = fallback;

        return fallback;
    }

    private MeshInstance3D? CreateBorders(int layer, IReadOnlyList<Vector3> solidCellCenters)
    {
        if (solidCellCenters.Count == 0)
            return null;

        var mesh = new ImmediateMesh();

        mesh.SurfaceBegin(
            Mesh.PrimitiveType.Lines,
            _borderMaterial
        );

        foreach (var solidCellCenter in solidCellCenters)
            AddCubeEdges(mesh, solidCellCenter);

        mesh.SurfaceEnd();

        return new MeshInstance3D
        {
            Name = $"Layer_{layer}_Borders",
            Mesh = mesh,
        };
    }

    private void AddCubeEdges(ImmediateMesh mesh, Vector3 center)
    {
        var half = CellSize / 2.0f + BorderOffset;

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

    private void AddLine(ImmediateMesh mesh,
                         Vector3 start,
                         Vector3 end)
    {
        mesh.SurfaceAddVertex(start);
        mesh.SurfaceAddVertex(end);
    }
}