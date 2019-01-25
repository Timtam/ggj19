using Godot;
using System;

public class PlayerCam : Camera
{
	Spatial target;
	Vector3 distanceToTarget = new Vector3(0, 1, 3);

	public override void _Ready()
	{
		target = (Spatial)GetNode("..");
	}

	public override void _Process(float delta)
	{
		var targetPos = target.GlobalTransform.origin;
		var targetVec = target.GlobalTransform.basis.Xform(distanceToTarget);
		LookAtFromPosition(targetPos + targetVec, targetPos, Vector3.Up);
	}
}
