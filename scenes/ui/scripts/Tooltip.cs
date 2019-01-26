using Godot;
using System;

public class Tooltip : PanelContainer
{
	public Label label;
	public override void _Ready()
	{
		label = (Label)GetNode("tooltip");
	}
}
