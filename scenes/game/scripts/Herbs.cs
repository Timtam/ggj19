using Godot;
using System;

public class Herbs : BaseInteractable
{
	public override void _Ready()
	{
		RegisterSignals();
	}

	public override Dialog GetInteractionDialog()
	{
		var text = "L: Kräuter für Zaubertränke und Rituale kann man immer gebrauchen.";
		if (DialogTriggers.HasListTree)
		{
			text += " [Diese brauche ich für Trank X.]";
		}
		return new Dialog
		{
			Text = text,
			Script = () =>
			{
				Inventory.Instance.AddItem(Items.GetItem("Kräuter"));
				this.QueueFree();
			}
		};
	}
}
