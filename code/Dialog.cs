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
}

public static class Dialogs
{
	public static Dialog HomeDestroyed = new Dialog
	{
		Text = "H: Der Sturm hat unserem Zuhause ganz schön zugesetzt. Wir müssen die Hütte wirklich reparieren.",
		Next = new Dialog
		{
			Text = "L: Meine Socken sind nass geworden. Das Dach ist undicht und letzte Nacht hat es hier reingeregnet.",
			Next = new Dialog
			{
				Text = "H: Wir brauchen dringend Holz und Steine um die Hütte zu reparieren, und für das Dach nehmen wir am besten trockenes Stroh.",
				Next = new Dialog
				{
					Text = "L: Das kann ich gerne alles besorgen gehen, aber wo finde ich die Sachen denn?",
					Next = new Dialog
					{
						Text = "H : Holz gibt es im [TODO]. Steine findest du im [TODO] und Gras für Stroh solltest du im [TODO] bekommen.",
						Next = new Dialog
						{
							Text = "L: Alles klar, ich schau mal, dass ich das alles besorgen kann.",
							Next = new Dialog
							{
								Text = "H: Dankeschön, komm einfach vorbei, wenn du Probleme beim Besorgen dieser Sachen hast, und pass auf die Mopps auf, die in der Gegend unterwegs sind."
							}
						}
					}
				}
			}
		}
	};

	public static Dialog SmallTree = new Dialog
	{
		Text = "L: Das ist der perfekte Baum zum Verbessern der Hauswände. Leider ist er noch viel zu klein. Ich werde mal die Hexe fragen, ob sie mir dabei helfen kann.",
		Script = () => { DialogTriggers.SmallTreeSeen = true; }
	};

	public static Dialog Witch = new Dialog
	{
		Text = "Hast du alles zusammen?",
		Options = new List<DialogOption>()
		{
			new DialogOption
			{
				Condition = () => DialogTriggers.SmallTreeSeen && !DialogTriggers.HasListTree,
				Text = "Baum",
				Next = new Dialog
				{
					Text = "L: Ich habe einen passenden Baum gefunden, aber er ist noch zu klein.",
					Next = new Dialog
					{
						Text = "H: Ich habe ein Rezept für einen Wachstumstrank, der dir hier helfen könnte. Ich gebe dir die Zutatenliste mit. Bringe mir die Zutaten einfach her und ich braue ihn dir.",
						Script = () =>
						{
							DialogTriggers.HasListTree = true;
							Inventory.Instance.AddItem(Items.GetItem("Zutatenliste (Wachstumstrank)"));
						}
					}
				}
			},
			new DialogOption
			{
				Condition = () => DialogTriggers.BigRockSeen && !DialogTriggers.HasListRock,
				Text = "Stein",
				Next = new Dialog
				{
					Text = "L: Ich habe einen passenden Stein gefunden, aber ich kann ihn nicht zertrümmern.",
					Next = new Dialog
					{
						Text = "H: Ich habe ein Rezept für einen Stärketrank, der dir hier helfen könnte. Ich gebe dir die Zutatenliste mit. Bringe mir die Zutaten einfach her und ich braue ihn dir.",
						Script = () =>
						{
							DialogTriggers.HasListRock = true;
							Inventory.Instance.AddItem(Items.GetItem("Zutatenliste (Stärketrank)"));
						}
					}
				}
			},
			new DialogOption
			{
				Condition = () => DialogTriggers.GrassSeen && !DialogTriggers.HasListGrass,
				Text = "Stroh",
				Next = new Dialog
				{
					Text = "L: Ich habe passendes Gras gefunden, aber das ist noch zu frisch.",
					Next = new Dialog
					{
						Text = "H: Ich habe ein Rezept für einen Trocknungtrank, der dir hier helfen könnte. Ich gebe dir die Zutatenliste mit. Bringe mir die Zutaten einfach her und ich braue ihn dir.",
						Script = () =>
						{
							DialogTriggers.HasListGrass = true;
							Inventory.Instance.AddItem(Items.GetItem("Zutatenliste (Trocknungstrank)"));
						}
					}
				}
			},
			new DialogOption
			{
				Condition = () => DialogTriggers.HasListTree && DialogTriggers.HasListRock && DialogTriggers.HasListGrass,
				Text = "Nein",
				Next = new Dialog
				{
					Text = "H: Dann komm gerne später wieder, wenn du alles beisammen hast. Ich räume hier noch weiter auf."
				}
			},
			new DialogOption
			{
				Condition = () => DialogTriggers.HasListTree && DialogTriggers.HasListRock && DialogTriggers.HasListGrass && Inventory.Instance.HasFinalItems(),
				Text = "Ja",
				Next = new Dialog
				{
					Text = "H: Vielen Dank für deine Mühe. Dann haben wir jetzt alles, um unsere kleine Hütte wieder auf Vordermann zu bringen."
				}
			},
		}
	};

	public static Dialog Death = new Dialog
	{
		Text = "Du wurdest zu Tode gemoppt."
	};
}
