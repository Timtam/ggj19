using Godot;
using System;

public class BerryBush : BaseInteractable
{
	public override void _Ready()
	{
		RegisterSignals();
	}

	public override Dialog GetInteractionDialog()
	{
		var text = "L: Aus diesen Beeren habe ich schon einmal einen duftenden Kuchen gebacken.";
		if (DialogTriggers.HasListTree) // TODO: Correct list
		{
			text += " Sie stehen auch auf meiner Liste.";
		}
		else
		{
			text += " Ich nehme ein paar mit.";
		}
		return new Dialog
		{
			Text = text,
			Script = () =>
			{
				Inventory.Instance.AddItem(Items.GetItem("Beeren"));
			}
		};
	}
}