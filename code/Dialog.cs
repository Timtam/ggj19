using Godot;
using System;
using System.Collections.Generic;

public class Dialog
{
	public IList<DialogOption> Options = new List<DialogOption>();
	public Dialog Next;
	public string Text = "";

	public delegate void ScriptHandler();
	public ScriptHandler Script;
}

public class DialogOption
{
	public string Text;
	public Dialog Next;
	public Func<bool> Condition;
}
