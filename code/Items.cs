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
			var dest = (new Vector2(40, 40) - image.GetSize()) / 2;
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
	public static Item[] all_items = new Item[] {
		new Item { Name = "Holz", IconPath = "res://assets/ui/item/Holz.png" },
		new Item { Name = "Steine", IconPath = "res://assets/ui/item/Stein.png" },
		new Item { Name = "Stroh", IconPath = "res://assets/ui/item/Stroh.png" },
		new Item { Name = "Pilze", IconPath = "res://assets/ui/item/Mitternachtspilz.png" },
		new Item { Name = "Wasser", IconPath = "res://assets/ui/item/Wasser.png" },
		new Item { Name = "Beeren", IconPath = "res://assets/ui/item/Beeren.png" },
		new Item { Name = "Kräuter", IconPath = "res://assets/ui/item/Krauter.png" },
		new Item { Name = "Federn", IconPath = "res://assets/ui/item/Feder_rot.png" },
		new Item { Name = "Kristall", IconPath = "res://assets/ui/item/Kristall_rosa.png" },
		new Item { Name = "Blüten", IconPath = "res://assets/ui/item/Blute.png" },
		new Item { Name = "Flügel", IconPath = "res://assets/ui/item/Flugel.png" },
		new Item { Name = "Zähne", IconPath = "res://assets/ui/item/Zahn.png" },
		new Item { Name = "Spinnenaugen", IconPath = "res://assets/ui/item/Auge.png" },
		new Item { Name = "Zutatenliste (Stärketrank)", IconPath = "res://assets/ui/item/Liste.png" },
		new Item { Name = "Zutatenliste (Wachstumstrank)", IconPath = "res://assets/ui/item/Liste.png" },
		new Item { Name = "Zutatenliste (Trocknungstrank)", IconPath = "res://assets/ui/item/Liste.png" },
	};

	public static Item GetItem(string name)
	{
		return all_items.First(i => i.Name == name);
	}
}
