using Godot;
using System;

public class Tooltip : PanelContainer
{
	public Label label;
	Vector2 pos = Vector2.Zero;
	public override void _Ready()
	{
		label = (Label)GetNode("tooltip");
	}

	public void DisplayWithText(string text)
	{
		label.Text = text;
		this.Visible = true;
	}

	public override void _Process(float delta)
	{
		if (!this.Visible) return;
		this.SetGlobalPosition(pos);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion motion)
		{
			pos = motion.GlobalPosition;
		}
	}
}
