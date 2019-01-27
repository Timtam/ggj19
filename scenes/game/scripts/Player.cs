using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player : KinematicBody
{
	const float MOUSE_SENSITIVITY = 0.0009f;
	const float ATTACK_DAMAGE = 35.0f;

	public float Health;

	Spatial target;
	Camera camera;
	Vector3 velocity = Vector3.Zero;
	MoveController controller;
	IList<IInteractable> closeInteractions = new List<IInteractable>();
	Area attackArea;
	AudioStreamPlayer fxPlayer;
	AudioStreamPlayer stepPlayer;

	AudioStream[] stepSounds = new AudioStream[10];
	AudioStream[] landingSounds = new AudioStream[2];
	bool jumping = false;

	public override void _Ready()
	{
		target = (Spatial)GetNode("target");
		camera = (PlayerCam)GetNode("target/camera");
		attackArea = (Area)GetNode("attack_area");
		fxPlayer = (AudioStreamPlayer)GetNode("fx_player");
		stepPlayer = (AudioStreamPlayer)GetNode("step_player");
		Input.SetMouseMode(Godot.Input.MouseMode.Captured);
		Health = 100;
		controller = new MoveController(this);

		for (int i = 0; i < stepSounds.Length; i++)
		{
			stepSounds[i] = ResourceLoader.Load($"res://sounds/footstep_{i + 1}.ogg") as AudioStream;
		}
		for (int i = 0; i < landingSounds.Length; i++)
		{
			landingSounds[i] = ResourceLoader.Load($"res://sounds/landing_{i + 1}.ogg") as AudioStream;
		}
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("interact"))
		{
			if (!closeInteractions.Any())
			{
				return;
			}
			var interaction = closeInteractions.OrderBy(i => (i as Spatial).GlobalTransform.origin.DistanceSquaredTo(this.GlobalTransform.origin)).First();
			this.GetGameWorld().DialogSystem.DisplayDialog(interaction.GetInteractionDialog());
			fxPlayer.Stream = ResourceLoader.Load("res://sounds/interact.ogg") as AudioStream;
			fxPlayer.Play();
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector3 moveVel = Vector3.Zero;

		var moveVec = new Vector2();
		if (Input.IsActionPressed("movement_forward"))
			moveVec.y -= 1;
		if (Input.IsActionPressed("movement_back"))
			moveVec.y += 1;
		if (Input.IsActionPressed("movement_left"))
			moveVec.x -= 1;
		if (Input.IsActionPressed("movement_right"))
			moveVec.x += 1;
		bool sprinting = Input.IsActionPressed("movement_sprint");
		if (moveVec.LengthSquared() > 0.1f)
		{
			if (!stepPlayer.Playing && IsOnFloor())
			{
				stepPlayer.Stream = stepSounds[new Random().Next(stepSounds.Length)];
				stepPlayer.Play();
			}
		}
		if (jumping && IsOnFloor())
		{
			stepPlayer.Stream = landingSounds[new Random().Next(landingSounds.Length)];
			stepPlayer.Play();
			jumping = false;
		}
		else if (!IsOnFloor())
		{
			jumping = true;
		}

		controller.PhysicsMove(delta, ref velocity, moveVec, moveBasis: this.GlobalTransform.basis, sprinting: Input.IsActionPressed("movement_sprint"), jump: Input.IsActionJustPressed("jump"));

		if (Input.IsActionJustPressed("attack"))
		{
			fxPlayer.Stream = ResourceLoader.Load("res://sounds/player_attack.ogg") as AudioStream;
			fxPlayer.VolumeDb = -3;
			fxPlayer.Play();
			var mopp = attackArea.GetOverlappingBodies().Select(b => b as Mopp).Where(b => b != null).FirstOrDefault();
			if (mopp != null)
			{
				mopp.PlayHitSound();
				mopp.Health -= ATTACK_DAMAGE;
				var dist = mopp.GlobalTransform.origin - this.GlobalTransform.origin;
				dist = dist.Normalized();
				dist.y += 1;
				dist = dist.Normalized();
				mopp.Velocity += dist * 7.0f;

				if (mopp.Health <= 0)
				{
					mopp.Die();
				}
			}
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion motion && Input.GetMouseMode() == Input.MouseMode.Captured)
		{
			target.RotateX(motion.Relative.y * MOUSE_SENSITIVITY * -1);
			this.RotateY(motion.Relative.x * MOUSE_SENSITIVITY * -1);

			var targetRotation = target.RotationDegrees;
			targetRotation.x = Mathf.Clamp(targetRotation.x, -70, 70);
			target.RotationDegrees = targetRotation;
		}
	}

	public void EnterInteractionArea(IInteractable i)
	{
		closeInteractions.Add(i);
	}

	public void ExitInteractionArea(IInteractable i)
	{
		closeInteractions.Remove(i);
	}
}
