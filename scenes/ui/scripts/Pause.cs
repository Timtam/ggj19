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
			UI.Instance.SetActive(UIPart.Pause, this.Visible);
		}
	}

	public void OnExitButtonPressed()
	{
		this.GetTree().Quit();
	}

	public void OnContinueButtonPressed()
	{
		this.Visible = false;
		UI.Instance.SetActive(UIPart.Pause, false);
	}
}
