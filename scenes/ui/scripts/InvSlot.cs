using Godot;
using System;

public class InvSlot : TextureRect
{
	public InvSlot()
	{
		Texture = ResourceLoader.Load("res://assets/ui/panel_woodPaperDetail.png") as Texture;
	}
}
