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

public static class DialogTriggers
{
	public static bool SmallTreeSeen = false;
	public static bool BigRockSeen = false;
	public static bool GrassSeen = false;
	public static bool HasListTree = false;
	public static bool HasListRock = false;
	public static bool HasListGrass = false;
	public static bool GrowthPotion = false;
	public static bool StrengthPotion = false;
	public static bool DryPotion = false;
	public static bool FirstTalk = true;
}
