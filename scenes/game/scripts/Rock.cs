using Godot;
using System;

public class Rock : BaseInteractable
{
	public override void _Ready()
	{
		RegisterSignals();
	}

	public override Dialog GetInteractionDialog()
	{
		if (DialogTriggers.StrengthPotion)
		{
			return new Dialog
			{
				Text = "L: Dieser Stein sieht gut aus, um damit die Wände zu verbessern. Mit dem Stärketrank kann ich mir jetzt Stücke mitnehmen.",
				Script = () =>
				{
					Inventory.Instance.AddItem(Items.GetItem("Steine"));
				}
			};
		}
		else
		{
			return new Dialog
			{
				Text = "L: Dieser Stein sieht gut aus, um damit die Wände zu verbessern. Leider bin ich nicht stark genug, um ihn in tragbare Stücke zu zerteilen. Vielleicht kann mir die Hexe dabei ja helfen.",
				Script = () =>
				{
					DialogTriggers.BigRockSeen = true;
				}
			};
		}
	}
}
