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
		if (DialogTriggers.HasListTree) // TODO: Correct list
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
			Script = () =>
			{
				Inventory.Instance.AddItem(Items.GetItem("Federn"));
			}
		};
	}
}
