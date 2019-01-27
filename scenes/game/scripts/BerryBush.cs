using Godot;
using System;

public class BerryBush : BaseInteractable
{
	public override void _Ready()
	{
		RegisterSignals();
	}

	public override Dialog GetInteractionDialog()
	{
		var text = "L: Aus diesen Beeren habe ich schon einmal einen duftenden Kuchen gebacken.";
		if (DialogTriggers.HasListTree)
		{
			text += " Sie stehen auch auf meiner Liste.";
		}
		else
		{
			text += " Ich nehme ein paar mit.";
		}
		return new Dialog
		{
			Text = text,
			Sounds = new string[]
			{
				"res://sounds/collect.ogg",
			},
			Script = () =>
			{
				Inventory.Instance.AddItem(Items.GetItem("Beeren"));
				this.Set("mesh", ResourceLoader.Load("res://assets/world/obj/Beerenbusch_alle.obj"));
				this.Enabled = false;
				GetNode("Area").EmitSignal("body_exited", this.GetGameWorld().Player);
			}
		};
	}
}
