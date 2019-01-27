using Godot;
using System;

public class MoveController
{
	public const float GRAVITY = -9.8f;
	public const float JUMP_SPEED = 7.0f;
	public const float ACCELERATION = 4.0f;
	public const float ACCELERATION_WALL = 1.0f;
	public const float DECELERATION = 16.0f;
	public const float MOVE_SPEED = 6.0f;
	public const float SPRINT_SPEED = 10.0f;

	KinematicBody body;

	public MoveController(KinematicBody body)
	{
		this.body = body;
	}

	public void PhysicsMove(float delta, ref Vector3 velocity, Vector2 moveVec, Basis moveBasis, bool sprinting = false, bool jump = false)
	{
		var moveVelocity = Vector3.Zero;

		moveVelocity += moveBasis.x.Normalized() * moveVec.x;
		moveVelocity += moveBasis.z.Normalized() * moveVec.y;
		moveVelocity.y = 0;
		moveVelocity = moveVelocity.Normalized() * (sprinting ? SPRINT_SPEED : MOVE_SPEED);

		var hVelocity = velocity;
		hVelocity.y = 0;
		var accel = (moveVelocity.Dot(hVelocity) > 0) ? (body.IsOnWall() ? ACCELERATION_WALL : ACCELERATION) : DECELERATION;
		hVelocity = hVelocity.LinearInterpolate(moveVelocity, accel * delta);

		if (body.IsOnFloor() && jump)
			velocity.y = JUMP_SPEED;

		velocity.y += GRAVITY * delta;
		velocity.x = hVelocity.x;
		velocity.z = hVelocity.z;
		velocity = body.MoveAndSlide(velocity, Vector3.Up, 0.05f, 4, Mathf.Deg2Rad(20));
	}
}
