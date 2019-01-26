using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player : KinematicBody
{
	const float GRAVITY = -9.8f;
	const float MOVE_SPEED = 6.0f;
	const float SPRINT_SPEED = 10.0f;
	const float MOUSE_SENSITIVITY = 0.0009f;
	const float JUMP_SPEED = 7.0f;

	public float Health;

	Spatial target;
	Camera camera;
	Vector3 velocity = Vector3.Zero;
	IList<IInteractable> closeInteractions = new List<IInteractable>();

	public override void _Ready()
	{
		target = (Spatial)GetNode("target");
		camera = (PlayerCam)GetNode("target/camera");
		Input.SetMouseMode(Godot.Input.MouseMode.Captured);
		Health = 100;
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

		var basis = this.GlobalTransform.basis;
		var moveVec = new Vector2();
		if (Input.IsActionPressed("movement_forward"))
			moveVec.y -= 1;
		if (Input.IsActionPressed("movement_back"))
			moveVec.y += 1;
		if (Input.IsActionPressed("movement_left"))
			moveVec.x -= 1;
		if (Input.IsActionPressed("movement_right"))
			moveVec.x += 1;
		moveVec = moveVec.Normalized();
		float speed = Input.IsActionPressed("movement_sprint") ? SPRINT_SPEED : MOVE_SPEED;
		moveVel += basis.x.Normalized() * moveVec.x * speed;
		moveVel += basis.z.Normalized() * moveVec.y * speed;

		if (this.IsOnFloor() && Input.IsActionJustPressed("jump"))
			velocity.y = JUMP_SPEED;

		velocity.y += GRAVITY * delta;
		if (this.IsOnFloor())
		{
			velocity.x = moveVel.x;
			velocity.z = moveVel.z;
		}
		velocity = MoveAndSlide(velocity, Vector3.Up, 0.05f, 4, Mathf.Deg2Rad(20));
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
