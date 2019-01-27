using Godot;
using System;

public class Grass : BaseInteractable
{
	public override void _Ready()
	{
		RegisterSignals();
	}

	public override Dialog GetInteractionDialog()
	{
		if (DialogTriggers.DryPotion)
		{
			return new Dialog
			{
				Text = "L: Das Gras hier wäre perfekt, um damit unser Dach zu reparieren. Mit dem Trocknungstrank kann ich daraus jetzt Stroh machen.",
				Script = () =>
				{
					Inventory.Instance.AddItem(Items.GetItem("Stroh"));
					this.QueueFree();
				}
			};
		}
		else
		{
			return new Dialog
			{
				Text = "L: Das Gras hier wäre perfekt, um damit unser Dach zu reparieren. Allerdings müsste es dafür noch getrocknet werden. Die Hexe hat bestimmt ein Trankrezept, um mir dabei zu helfen.",
				Script = () =>
				{
					DialogTriggers.GrassSeen = true;
				}
			};
		}
	}
}
