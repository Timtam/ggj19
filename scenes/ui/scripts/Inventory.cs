using Godot;
using System;
using System.Linq;

public class Inventory : Panel
{
	GridContainer invGrid;
	InvSlot[] invSlots;
	bool isInInventory = false;

	public Tooltip Tooltip;

	public static Inventory Instance;

	public override void _Ready()
	{
		invGrid = (GridContainer)GetNode("inv_grid");
		Tooltip = (Tooltip)GetNode("tooltip_layer/tooltip_panel");
		invSlots = new InvSlot[20];
		for (int i = 0; i < invSlots.Length; i++)
		{
			invSlots[i] = new InvSlot();
			invSlots[i].Theme = invGrid.Theme;
			invGrid.AddChild(invSlots[i]);
		}

		Instance = this;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey key && key.IsActionPressed("inventory"))
		{
			var tree = this.GetTree();
			if (!isInInventory && tree.Paused)
			{
				return;
			}
			else if (!isInInventory)
			{
				tree.Paused = true;
				this.Visible = true;
				this.isInInventory = true;
				Input.SetMouseMode(Input.MouseMode.Visible);
			}
			else
			{
				tree.Paused = false;
				this.Visible = false;
				this.isInInventory = false;
				Input.SetMouseMode(Input.MouseMode.Captured);
			}
		}
	}

	public void AddItem(Item item, int count = 1)
	{
		var slot = invSlots.FirstOrDefault(s => s.ContainsItem(item)) ?? invSlots.FirstOrDefault(s => s.IsEmpty());
		if (slot == null)
		{
			GD.PrintErr($"No slot for item {item.Name}, discard");
			return;
		}

		slot.AddItem(item, count);
	}

	public bool HasFinalItems()
	{
		bool hasTree = false, hasRock = false, hasGrass = false;
		foreach (var slot in invSlots)
		{
			if (slot.ContainsItem(Items.GetItem("Holz"))) hasTree = true;
			if (slot.ContainsItem(Items.GetItem("Steine"))) hasRock = true;
			if (slot.ContainsItem(Items.GetItem("Stroh"))) hasGrass = true;
		}
		return hasTree && hasRock && hasGrass;
	}
}
