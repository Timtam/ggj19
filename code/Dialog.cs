using Godot;
using System;
using System.Collections.Generic;

public class Dialog
{
	public IDictionary<string, Dialog> Options = new System.Collections.Generic.Dictionary<string, Dialog>();
	public Dialog Next;
	public string Text = "";
}
