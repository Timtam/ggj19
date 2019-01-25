using Godot;
using System;

public class Player : KinematicBody
{
	const float GRAVITY = -9.8f;
	const float MOVE_SPEED = 10.0f;
	const float MOUSE_SENSITIVITY = 0.0009f;
	const float JUMP_SPEED = 8.0f;

	Spatial target;
	Camera camera;
	Vector3 velocity = Vector3.Zero;

	public override void _Ready()
	{
		target = (Spatial)GetNode("target");
		camera = (PlayerCam)GetNode("target/camera");
		Input.SetMouseMode(Godot.Input.MouseMode.Captured);
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
		moveVel += basis.x.Normalized() * moveVec.x * MOVE_SPEED;
		moveVel += basis.z.Normalized() * moveVec.y * MOVE_SPEED;

		if (this.IsOnFloor() && Input.IsActionJustPressed("jump"))
			velocity.y = JUMP_SPEED;

		velocity.y += GRAVITY * delta;
		velocity.x = moveVel.x;
		velocity.z = moveVel.z;
		velocity = MoveAndSlide(velocity, Vector3.Up);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion motion && Input.GetMouseMode() == Input.MouseMode.Captured)
		{
			this.RotateY(motion.Relative.x * MOUSE_SENSITIVITY * -1);
			target.RotateX(motion.Relative.y * MOUSE_SENSITIVITY * -1);

			var targetRotation = target.RotationDegrees;
			targetRotation.x = Mathf.Clamp(targetRotation.x, -70, 70);
			target.RotationDegrees = targetRotation;
		}
	}
}
