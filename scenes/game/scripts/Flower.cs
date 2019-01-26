using Godot;
using System;

public class Flower : BaseInteractable
{
	public override void _Ready()
	{
		RegisterSignals();
	}

	public override Dialog GetInteractionDialog()
	{
		var text = "L: Was für eine wunderschöne Blüte. Die nehme ich mit nach Hause.";
		if (DialogTriggers.HasListTree) // TODO: Correct list
		{
			text = "L: Was für eine wunderschöne Blüte. Normalerweise würde ich sie für einen Blumenstrauß mit nach Hause nehmen, heute wird sie wohl eher als Zaubertrankzutat herhalten müssen.";
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
