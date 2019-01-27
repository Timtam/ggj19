using Godot;
using System;

public class MoppDrop : RigidBody
{
	public string ItemName;

	Area pickupArea;
	MeshInstance meshInstance;

	public override void _Ready()
	{
		pickupArea = (Area)GetNode("pickup_area");
		meshInstance = (MeshInstance)GetNode("mesh");

		pickupArea.Connect("body_entered", this, nameof(OnBodyEntered));
	}

	public void OnBodyEntered(CollisionObject other)
	{
		if (other is Player player)
		{
			this.QueueFree();
			Inventory.Instance.AddItem(Items.GetItem(ItemName), 1);
		}
	}
}
