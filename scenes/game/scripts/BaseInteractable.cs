using Godot;
using System;

public abstract class BaseInteractable : Spatial, IInteractable
{
	protected int enterCounter = 0;
	public bool Enabled = true;

	protected void RegisterSignals()
	{
		var area = (Area)GetNode("Area");
		area.Connect("body_entered", this, nameof(OnBodyEntered));
		area.Connect("body_exited", this, nameof(OnBodyExited));
	}

	public void OnBodyEntered(CollisionObject other)
	{
		if (other is Player player && Enabled)
		{
			player.EnterInteractionArea(this);
			enterCounter++;
		}
	}

	public void OnBodyExited(CollisionObject other)
	{
		if (other is Player player && (Enabled || enterCounter > 0))
		{
			player.ExitInteractionArea(this);
			enterCounter--;
		}
	}

	public abstract Dialog GetInteractionDialog();
}
