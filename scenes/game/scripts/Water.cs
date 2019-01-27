using Godot;
using System;

public class Water : BaseInteractable
{
	public override void _Ready()
	{
		RegisterSignals();
	}

	public override Dialog GetInteractionDialog()
	{
		return new Dialog
		{
			Text = "L: Das klare Flusswasser benutzen wir normalerweise zum Kochen von leckeren Kräutertees. Aber es ist auch die perfekte Basis für einen Zaubertrank.",
			Sounds = new string[]
			{
				"res://sounds/collect.ogg",
			},
			Script = () =>
			{
				Inventory.Instance.AddItem(Items.GetItem("Wasser"));
			}
		};
	}
}
