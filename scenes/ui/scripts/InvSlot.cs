using Godot;
using System;

public class InvSlot : TextureRect
{
	public InvItem InvItem;

	public InvSlot()
	{
		Texture = ResourceLoader.Load("res://assets/ui/panel_woodPaperDetail.png") as Texture;
	}

	public bool ContainsItem(Item item)
	{
		if (InvItem == null) return item == null;
		return InvItem.Item.Name == item.Name && InvItem.Count > 0;
	}

	public bool IsEmpty()
	{
		return InvItem == null;
	}

	public void AddItem(Item item, int count)
	{
		if (InvItem == null)
		{
			InvItem = new InvItem(item, count, this);
			InvItem.Theme = this.Theme;
			AddChild(InvItem);
		}
		else
		{
			if (InvItem.Item.Name != item.Name)
			{
				GD.PrintErr($"Item doesn't fit, discarding");
				return;
			}
			InvItem.Count += count;
		}
	}

	public void RemoveItem(Item item, int count)
	{
		if (InvItem == null)
		{
			GD.PrintErr($"Can't remove item from empty slot.");
		}
		else
		{
			if (InvItem.Item.Name != item.Name)
			{
				GD.PrintErr($"Item doesn't fit, discarding");
				return;
			}
			if (InvItem.Count < count)
			{
				GD.PrintErr($"Don't have enough items.");
				return;
			}
			InvItem.Count -= count;
			if (InvItem.Count == 0)
			{
				InvItem.QueueFree();
				InvItem = null;
			}
		}
	}
}
