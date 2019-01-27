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
				this.Visible = true;
				this.isInInventory = true;
				UI.Instance.SetActive(UIPart.Inventory, true);
			}
			else
			{
				this.Visible = false;
				this.isInInventory = false;
				UI.Instance.SetActive(UIPart.Inventory, false);
				Tooltip.Visible = false;
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

	public void RemoveItems(params string[] itemNames)
	{
		foreach (var name in itemNames)
		{
			var item = Items.GetItem(name);
			foreach (var slot in invSlots)
			{
				if (slot.ContainsItem(item))
					slot.RemoveItem(item, 1);
			}
		}
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

	public bool HasItems(params string[] itemNames)
	{
		var has = new bool[itemNames.Length];
		var items = itemNames.Select(n => Items.GetItem(n)).ToArray();
		foreach (var slot in invSlots)
		{
			for (int i = 0; i < items.Length; i++)
			{
				if (slot.ContainsItem(items[i])) has[i] = true;
			}
		}
		return has.All(b => b);
	}
}
