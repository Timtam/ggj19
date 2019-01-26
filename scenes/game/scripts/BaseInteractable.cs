using Godot;
using System;

public abstract class BaseInteractable : Spatial, IInteractable
{
	protected void RegisterSignals()
	{
		var area = (Area)GetNode("Area");
		area.Connect("body_entered", this, nameof(OnBodyEntered));
		area.Connect("body_exited", this, nameof(OnBodyExited));
	}

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
