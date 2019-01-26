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

	public static Dialog BigRock = new Dialog
	{
		Text = "L: Dieser Stein sieht gut aus, um damit die Wände zu verbessern. Leider bin ich nicht stark genug, um ihn in tragbare Stücke zu zerteilen. Vielleicht kann mir die Hexe dabei ja helfen.",
		Script = () => { DialogTriggers.BigRockSeen = true; }
	};

	public static Dialog Grass = new Dialog
	{
		Text = "L: Das Gras hier wäre perfekt, um damit unser Dach zu reparieren. Allerdings müsste es dafür noch getrocknet werden. Die Hexe hat bestimmt ein Trankrezept, um mir dabei zu helfen.",
		Script = () => { DialogTriggers.GrassSeen = true; }
	};

	public static Dialog Water = new Dialog
	{
		Text = "L: Das klare Flusswasser benutzen wir normalerweise zum Kochen von leckeren Kräutertees. Aber es ist auch die perfekte Basis für einen Zaubertrank."
	};

	public static Dialog Berries = new Dialog
	{
		Script = () =>
		{
			Berries.Text = "L: Aus diesen Beeren habe ich schon einmal einen duftenden Kuchen gebacken.";
			if (DialogTriggers.HasListTree) // TODO: Correct list
			{
				Berries.Text += " Sie stehen auch auf meiner Liste.";
			}
			else
			{
				Berries.Text += " Ich nehme ein paar mit.";
			}
		}
	};

	public static Dialog Feather = new Dialog
	{
		Script = () =>
		{
			Feather.Text = "L: Ein paar rote Federn, die wohl ein Rotkehlchen hier verloren hat.";
			if (DialogTriggers.HasListTree) // TODO: Correct list
			{
				Feather.Text += " Rote Federn, roter Trank.";
			}
			else
			{
				Feather.Text += " Ich nehme sie mit.";
			}
		}
	};

	public static Dialog Crystal = new Dialog
	{
		Script = () =>
		{
			Crystal.Text = "L: Dieser leuchtende Kristall nutzt die Hexe oft als Zutat für alle möglichen Zaubersprüche- und tränke.";
			if (DialogTriggers.HasListTree) // TODO: Correct list
			{
				Crystal.Text += " Der steht auch auf der Zutatenliste.";
			}
			else
			{
				Crystal.Text += " Sie hat mir damit sogar schonmal die Zukunft vorhergesagt.";
			}
		}
	};

	public static Dialog Flower = new Dialog
	{
		Script = () =>
		{
			Flower.Text = "L: Was für eine wunderschöne Blüte. Die nehme ich mit nach Hause.";
			if (DialogTriggers.HasListTree) // TODO: Correct list
			{
				Flower.Text = "L: Was für eine wunderschöne Blüte. Normalerweise würde ich sie für einen Blumenstrauß mit nach Hause nehmen, heute wird sie wohl eher als Zaubertrankzutat herhalten müssen.";
			}
		}
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
