using Godot;
using System;

public abstract class BaseInteractable : Spatial, IInteractable
{
	public void OnBodyEntered(CollisionObject other)
	{
		if (other is Player player)
		{
			player.EnterInteractionArea(this);
		}
	}

	public void OnBodyExited(CollisionObject other)
	{
		if (other is Player player)
		{
			player.ExitInteractionArea(this);
		}
	}

	public abstract Dialog GetInteractionDialog();
}
