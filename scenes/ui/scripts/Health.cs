using Godot;
using System;

public class Health : TextureProgress
{
	Texture barGreen;
	Texture barYellow;
	Texture barRed;

	public override void _Ready()
	{
		barGreen = ResourceLoader.Load("res://assets/ui/barHorizontal_green.png") as Texture;
		barYellow = ResourceLoader.Load("res://assets/ui/barHorizontal_yellow.png") as Texture;
		barRed = ResourceLoader.Load("res://assets/ui/barHorizontal_red.png") as Texture;
	}

	public override void _Process(float delta)
	{
		var player = this.GetGameWorld().Player;

		this.MaxValue = 100;
		this.Value = player.Health;
		var frac = this.Value / this.MaxValue;
		if (frac >= 0.6f)
		{
			this.TextureProgress_ = barGreen;
		}
		else if (frac >= 0.3f)
		{
			this.TextureProgress_ = barYellow;
		}
		else
		{
			this.TextureProgress_ = barRed;
		}
	}
}
