using Godot;
using System;
using System.Collections.Generic;

public class Mushroom : BaseInteractable
{
	public override void _Ready()
	{
	}

	public override Dialog GetInteractionDialog()
	{
		return new Dialog
		{
			Text = "Ein Pilz.\nAufheben?",
			Options = new List<DialogOption> {
				new DialogOption { Text = "Ja", Next = new Dialog { Text = "Du hebst den Pilz auf.", Script = Script_Pickup } },
				new DialogOption { Text = "Nein" },
				new DialogOption { Text = "Vielleicht", Condition = () => { return new Random().NextDouble() > 0.5; } },
			}
		};
	}

	private void Script_Pickup()
	{
		this.QueueFree();
		this.GetGameWorld().Inventory.AddItem(Items.GetItem("Pilze"));
	}
}
