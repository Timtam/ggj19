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
		if (DialogTriggers.HasListGrass)
		{
			text += " Diese brauche ich für den Trockungstrank.";
		}
		return new Dialog
		{
			Text = text,
			Sounds = new string[]
			{
				"res://sounds/collect.ogg",
			},
			Script = () =>
			{
				Inventory.Instance.AddItem(Items.GetItem("Kräuter"));
				this.QueueFree();
			}
		};
	}
}
