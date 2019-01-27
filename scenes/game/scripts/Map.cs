using Godot;
using System;
using System.Linq;

public class Map : Spatial
{
	public override void _Ready()
	{
		foreach (var child in GetChildren().Concat(GetNode("terrain").GetChildren()))
		{
			if (!(child is MeshInstance mesh)) continue;
			mesh.CreateTrimeshCollision();
		}
	}
}
