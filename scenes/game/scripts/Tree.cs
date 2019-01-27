using Godot;
using System;

public class Tree : BaseInteractable
{
	MeshInstance smallMesh;
	MeshInstance bigMesh;
	bool isBig = false;

	public override void _Ready()
	{
		RegisterSignals();

		smallMesh = (MeshInstance)GetNode("tree_small");
		bigMesh = (MeshInstance)GetNode("tree_big");
		bigMesh.Visible = false;
	}

	public override Dialog GetInteractionDialog()
	{
		if (isBig)
		{
			return new Dialog
			{
				Text = "L: Das ist der perfekte Baum zum Verbessern der Hauswände. Jetzt kann ich Holz mitnehmen.",
				Script = () =>
				{
					Inventory.Instance.AddItem(Items.GetItem("Holz"));
					this.QueueFree();
				}
			};
		}
		else if (DialogTriggers.GrowthPotion)
		{
			return new Dialog
			{
				Text = "L: Das ist der perfekte Baum zum Verbessern der Hauswände. Mit dem Wachstumstrank kann ich ihn jetzt wachsen lassen.",
				Script = () =>
				{
					isBig = true;
					bigMesh.Visible = true;
					smallMesh.Visible = false;
				}
			};
		}
		else
		{
			return new Dialog
			{
				Text = "L: Das ist der perfekte Baum zum Verbessern der Hauswände. Leider ist er noch viel zu klein. Ich werde mal die Hexe fragen, ob sie mir dabei helfen kann.",
				Script = () =>
				{
					DialogTriggers.SmallTreeSeen = true;
				}
			};
		}
	}
}
