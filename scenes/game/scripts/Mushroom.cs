using Godot;
using System;

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
			Options = new System.Collections.Generic.Dictionary<string, Dialog> {
				{ "Ja", new Dialog { Text = "Du hebst den Pilz auf.", Script = Script_Pickup } },
				{ "Nein", null },
			}
		};
	}

	private void Script_Pickup()
	{
		this.QueueFree();
		// TODO: Add to inventory
	}
}
