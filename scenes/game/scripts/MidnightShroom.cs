using Godot;
using System;

public class MidnightShroom : BaseInteractable
{
	public override void _Ready()
	{
		RegisterSignals();
	}

	public override Dialog GetInteractionDialog()
	{
		var text = "L: Diesen schönen blauen Mitternachtspilz kenne ich schon aus meinem Grimoire.";
		if (DialogTriggers.HasListTree) // TODO: Correct list
		{
			text += " Der steht außerdem auf meiner Liste.";
		}
		return new Dialog
		{
			Text = text,
			Script = () =>
			{
				Inventory.Instance.AddItem(Items.GetItem("Mitternachtspilz"));
				this.QueueFree();
			}
		};
	}
}