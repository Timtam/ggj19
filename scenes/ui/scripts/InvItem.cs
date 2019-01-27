using Godot;
using System;

public class InvItem : TextureRect
{
	public Item Item;
	public int Count;
	public InvSlot Slot;

	Label countLabel;

	public InvItem(Item item, int count, InvSlot slot)
	{
		this.Slot = slot;
		this.Item = item;
		this.Count = count;
		this.Texture = item.GetIcon();
		this.HintTooltip = item.Name;
		this.MarginLeft = 12;
		this.MarginTop = 12;

		this.Connect("mouse_entered", this, nameof(OnMouseEntered));
		this.Connect("mouse_exited", this, nameof(OnMouseExited));

		countLabel = new Label();
		countLabel.SetAlign(Label.AlignEnum.Right);
		countLabel.SetValign(Label.VAlign.Bottom);
		countLabel.AnchorRight = 1;
		countLabel.MarginRight = 2;
		countLabel.AnchorBottom = 1;
		countLabel.MarginBottom = 2;
		countLabel.AddColorOverride("font_color", new Color(0, 0, 0, 1));
		countLabel.AddFontOverride("font", ResourceLoader.Load("res://assets/ui/small_font.tres") as Font);
		AddChild(countLabel);
	}

	public override void _Process(float delta)
	{
		countLabel.Text = $"{Count}";
	}

	public void OnMouseEntered()
	{
		this.GetGameWorld().Inventory.Tooltip.DisplayWithText(Item.Name);
	}

	public void OnMouseExited()
	{
		this.GetGameWorld().Inventory.Tooltip.Visible = false;
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton button)
		{
			if (button.Pressed && button.ButtonIndex == (int)ButtonList.Left)
			{
				switch (Item.Name)
				{
					case "Zutatenliste (Wachstumstrank)":
						this.GetGameWorld().DialogSystem.DisplayDialog(new Dialog { Text = "Für den Wachstumstrank benötigt man:\n- Wasser\n- Mop-Flügel\n- Einige Beeren\n- Einen Kristall" });
						break;
					case "Zutatenliste (Stärketrank)":
						this.GetGameWorld().DialogSystem.DisplayDialog(new Dialog { Text = "Für den Stärketrank benötigt man:\n- Wasser\n- Mop-Augen\n- Einen Mitternachtspilz\n- Eine Blüte" });
						break;
					case "Zutatenliste (Trocknungstrank)":
						this.GetGameWorld().DialogSystem.DisplayDialog(new Dialog { Text = "Für den Trocknungstrank benötigt man:\n- Wasser\n- Mop-Zähne\n- Einige Kräuter\n- Einige Federn" });
						break;
				}
			}
		}
	}
}
