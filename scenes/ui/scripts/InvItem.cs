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
}