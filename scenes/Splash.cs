using Godot;
using System;

public class Splash : TextureRect
{
	public void OnTimerTimeout()
	{
		this.GetTree().ChangeScene("res://scenes/game.tscn");
	}
}
