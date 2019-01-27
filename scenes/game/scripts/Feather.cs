using Godot;
using System;

public class Feather : BaseInteractable
{
	public override void _Ready()
	{
		RegisterSignals();
	}

	public override Dialog GetInteractionDialog()
	{
		var text = "L: Ein paar rote Federn, die wohl ein Rotkehlchen hier verloren hat.";
		if (DialogTriggers.HasListGrass)
		{
			text += " Rote Federn, roter Trank.";
		}
		else
		{
			text += " Ich nehme sie mit.";
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
				Inventory.Instance.AddItem(Items.GetItem("Federn"));
				this.QueueFree();
			}
		};
	}
}
