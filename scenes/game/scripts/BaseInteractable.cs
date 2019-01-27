using Godot;
using System;
using System.Linq;

public abstract class BaseInteractable : Spatial, IInteractable
{
	protected int enterCounter = 0;
	public bool Enabled = true;

	Sprite3D billboard = null;

	protected void RegisterSignals()
	{
		foreach (var area in GetChildren().Select(o => o as Area).Where(a => a != null))
		{
			area.Connect("body_entered", this, nameof(OnBodyEntered));
			area.Connect("body_exited", this, nameof(OnBodyExited));
		}
		billboard = GetNode("billboard") as Sprite3D;
		billboard.Visible = false;
	}

	public void OnBodyEntered(CollisionObject other)
	{
		if (other is Player player && Enabled)
		{
			if (enterCounter == 0)
			{
				player.EnterInteractionArea(this);
				billboard.Visible = true;
			}
			enterCounter++;
		}
	}

	public void OnBodyExited(CollisionObject other)
	{
		if (other is Player player && (Enabled || enterCounter > 0))
		{
			enterCounter--;
			if (enterCounter == 0)
			{
				player.ExitInteractionArea(this);
				billboard.Visible = false;
			}
		}
	}

	public abstract Dialog GetInteractionDialog();
}
