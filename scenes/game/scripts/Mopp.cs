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
		this.QueueFree();
		var itemSpawn = GlobalTransform.origin;
		itemSpawn.y += 1;
		var moppDropScene = ResourceLoader.Load("res://scenes/game/mopp_drop.tscn") as PackedScene;
		var moppDrop = moppDropScene.Instance() as MoppDrop;
		moppDrop.SetTranslation(itemSpawn);
		// TODO: Set mesh and item name in a useful way
		moppDrop.ItemName = (new[] { "Flügel", "Zähne", "Spinnenaugen" })[new Random().Next(3)];
		GetNode("../drop").AddChild(moppDrop);
	}
}
