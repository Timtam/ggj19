using Godot;
using System;
using System.Linq;

public class Item
{
	public string Name;
	public string IconPath;
	Texture icon;

	public Texture GetIcon()
	{
		if (icon == null)
		{
			var rawIcon = ResourceLoader.Load(IconPath ?? "res://assets/ui/item/question.png") as Texture;
			var image = rawIcon.GetData();
			image.Lock();
			var scaleFactor = 40.0f / Math.Max(image.GetHeight(), image.GetWidth());
			image.Resize(Mathf.RoundToInt(image.GetWidth() * scaleFactor), Mathf.RoundToInt(image.GetHeight() * scaleFactor), Image.Interpolation.Cubic);
			image.Unlock();
			var newImage = new Image();
			newImage.Create(40, 40, false, Image.Format.Rgba8);
			newImage.Lock();
			var dest = (image.GetSize() - new Vector2(40, 40)) / 2;
			newImage.BlitRect(image, new Rect2(Vector2.Zero, image.GetSize()), dest);
			newImage.Unlock();

			var tex = new ImageTexture();
			tex.CreateFromImage(newImage);
			icon = tex;

		}
		return icon;
	}
}

public class Items
{
	static Item[] all_items = new Item[] {
		new Item { Name = "Holz" },
		new Item { Name = "Steine" },
		new Item { Name = "Stroh" },
		new Item { Name = "Pilze" },
		new Item { Name = "Wasser" },
		new Item { Name = "Beeren" },
		new Item { Name = "Kräuter" },
		new Item { Name = "Federn" },
		new Item { Name = "Kristall" },
		new Item { Name = "Blüten" },
		new Item { Name = "Flügel" },
		new Item { Name = "Zähne" },
		new Item { Name = "Spinnenaugen" },
	};

	public static Item GetItem(string name)
	{
		return all_items.First(i => i.Name == name);
	}
}
