using Godot;
using System;

public class Crystal : BaseInteractable
{
	public override void _Ready()
	{
		RegisterSignals();
	}

	public override Dialog GetInteractionDialog()
	{
		var text = "L: Dieser leuchtende Kristall nutzt die Hexe oft als Zutat für alle möglichen Zaubersprüche- und tränke.";
		if (DialogTriggers.HasListTree)
		{
			text += " Der steht auch auf der Zutatenliste.";
		}
		else
		{
			text += " Sie hat mir damit sogar schonmal die Zukunft vorhergesagt.";
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
				Inventory.Instance.AddItem(Items.GetItem("Kristall"));
				this.QueueFree();
			}
		};
	}
}
