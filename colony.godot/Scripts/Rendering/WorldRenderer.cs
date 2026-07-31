using Godot;

public partial class WorldRenderer : Node3D
{
	private readonly BoxMesh _cubeMesh = new();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GenerateFloor();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void GenerateFloor()
	{
		for (int x = 0; x < 10; x++)
		{
			for (int z = 0; z < 10; z++)
			{
				AddChild(CreateCube(new Vector3(x, 0, z)));
			}
		}
	}
	
	private MeshInstance3D CreateCube(Vector3 position)
	{
		return new MeshInstance3D
		{
			Name = $"Cube_{position.X}_{position.Z}",
			Mesh = _cubeMesh,
			Position = position
		};
	}
}
