using Godot;
using System;

class Util
{
}

public static class Extensions
{
	static GameWorld world = null;
	public static GameWorld GetGameWorld(this Node node)
	{
		if (world == null)
		{
			world = (GameWorld)node.GetNode("/root/world");
			world.TreeExiting += (w) =>
			{
				if (w == world) world = null;
			};
		}
		return world;
	}
}
