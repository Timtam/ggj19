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
		return InvItem.Item.Name == item.Name;
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
}
