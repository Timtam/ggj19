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

	public override void _Ready()
	{
		target = (Spatial)GetNode("target");
		camera = (PlayerCam)GetNode("target/camera");
		attackArea = (Area)GetNode("attack_area");
		Input.SetMouseMode(Godot.Input.MouseMode.Captured);
		Health = 100;
		controller = new MoveController(this);
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

		controller.PhysicsMove(delta, ref velocity, moveVec, moveBasis: this.GlobalTransform.basis, sprinting: Input.IsActionPressed("movement_sprint"), jump: Input.IsActionJustPressed("jump"));

		if (Input.IsActionJustPressed("attack"))
		{
			var mopp = attackArea.GetOverlappingBodies().Select(b => b as Mopp).Where(b => b != null).FirstOrDefault();
			if (mopp != null)
			{
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
