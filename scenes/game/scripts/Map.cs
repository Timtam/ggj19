using Godot;
using System;

public class Map : Spatial
{
	public override void _Ready()
	{
		foreach (var child in GetChildren())
		{
			if (!(child is MeshInstance mesh)) continue;
			mesh.CreateTrimeshCollision();
		}
	}
}
