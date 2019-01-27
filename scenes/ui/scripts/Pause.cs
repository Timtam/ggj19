using Godot;
using System;

public class Pause : HBoxContainer
{
	public override void _Ready()
	{
	}

	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			this.Visible = !this.Visible;
			this.GetTree().Paused = this.Visible;
			if (this.Visible)
			{
				Input.SetMouseMode(Input.MouseMode.Visible);
			}
			else
			{
				Input.SetMouseMode(Input.MouseMode.Captured);
			}
		}
	}

	public void OnExitButtonPressed()
	{
		this.GetTree().Quit();
	}

	public void OnContinueButtonPressed()
	{
		this.Visible = false;
		this.GetTree().Paused = false;
		Input.SetMouseMode(Input.MouseMode.Captured);
	}
}
