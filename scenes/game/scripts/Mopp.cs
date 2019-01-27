using Godot;
using System;

public class Mopp : KinematicBody
{
	Player player;
	MoveController controller;
	public float Health;
	public Vector3 Velocity;

	public override void _Ready()
	{
		player = this.GetGameWorld().Player;
		Health = 100;
		controller = new MoveController(this);
	}

	public override void _PhysicsProcess(float delta)
	{
		controller.PhysicsMove(delta, ref Velocity, moveVec: Vector2.Zero, moveBasis: this.GlobalTransform.basis);
	}

	public void Die()
	{

	}
}
