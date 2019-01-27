using Godot;
using System;
using System.Linq;

[Flags]
public enum UIPart
{
	None = 0x00,
	Dialog = 0x01,
	Inventory = 0x02,
	Pause = 0x04,
}

public class UI : Control
{
	public static UI Instance;
	UIPart activeParts = UIPart.None;

	public override void _Ready()
	{
		Instance = this;
	}

	public void SetActive(UIPart part, bool active)
	{
		if (active)
		{
			activeParts |= part;
		}
		else
		{
			activeParts &= ~part;
		}

		GD.Print($"changed {part} to {active}, now active: {activeParts}");

		if (activeParts == UIPart.None)
		{
			this.GetTree().Paused = false;
			Input.SetMouseMode(Input.MouseMode.Captured);
		}
		else
		{
			this.GetTree().Paused = true;
			Input.SetMouseMode(Input.MouseMode.Visible);
		}

		if (IsActive(UIPart.Pause))
		{
			this.PauseMode = PauseModeEnum.Stop;
		}
		else
		{
			this.PauseMode = PauseModeEnum.Process;
		}
	}

	public bool IsActive(UIPart part) => (activeParts & part) != 0;
}
