using Godot;
using System;

public class Mopp : KinematicBody
{
	Player player;
	MoveController controller;
	AudioStreamPlayer3D fxPlayer;
	public float Health;
	public Vector3 Velocity;

	public override void _Ready()
	{
		fxPlayer = (AudioStreamPlayer3D)GetNode("fx_player");
		Health = 100;
		controller = new MoveController(this);
	}

	public override void _PhysicsProcess(float delta)
	{
		controller.PhysicsMove(delta, ref Velocity, moveVec: Vector2.Zero, moveBasis: this.GlobalTransform.basis);
	}

	public void PlayHitSound()
	{
		fxPlayer.Stream = ResourceLoader.Load($"res://sounds/mop_hit_{new Random().Next(1, 4)}.ogg") as AudioStream;
		fxPlayer.Play();
	}

	public void Die()
	{
		fxPlayer.Stream = ResourceLoader.Load($"res://sounds/mop_die.ogg") as AudioStream;
		fxPlayer.Play();
		this.QueueFree();
		var itemSpawn = GlobalTransform.origin;
		itemSpawn.y += 1;
		var moppDropScene = ResourceLoader.Load("res://scenes/game/mopp_drop.tscn") as PackedScene;
		var moppDrop = moppDropScene.Instance() as MoppDrop;
		moppDrop.SetTranslation(itemSpawn);
		moppDrop.ItemName = (new[] { "Flügel", "Zähne", "Spinnenaugen" })[new Random().Next(3)];
		switch (moppDrop.ItemName)
		{
			case "Flügel":
				moppDrop.MeshPath = "res://assets/world/obj/Flügel.obj";
				break;
			case "Zähne":
				moppDrop.MeshPath = "res://assets/world/obj/Zahn.obj";
				break;
			case "Spinnenaugen":
				moppDrop.MeshPath = "res://assets/world/obj/Auge.obj";
				break;
		}
		GetNode("../drop").AddChild(moppDrop);
	}
}
