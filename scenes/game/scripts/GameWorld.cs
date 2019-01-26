using Godot;
using System;

public class GameWorld : Spatial
{
	public delegate void TreeExitingEventHandler(GameWorld world);
	public event TreeExitingEventHandler TreeExiting;

	public DialogBox DialogSystem;
	public Player Player;

	public override void _Ready()
	{
		DialogSystem = (DialogBox)GetNode("ui/dialog_box");
		Player = (Player)GetNode("player");
	}

	public void OnTreeExiting()
	{
		TreeExiting?.Invoke(this);
	}
}
