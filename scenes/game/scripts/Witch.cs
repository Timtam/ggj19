using Godot;
using System;
using System.Collections.Generic;

public class Witch : BaseInteractable
{
	public override void _Ready()
	{
		RegisterSignals();
	}

	public override Dialog GetInteractionDialog()
	{
		if (DialogTriggers.FirstTalk)
		{
			return new Dialog
			{
				Text = "H: Der Sturm hat unserem Zuhause ganz schön zugesetzt. Wir müssen die Hütte wirklich reparieren.",
				Script = () => { DialogTriggers.FirstTalk = false; },
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
								Text = "H : Gutes Holz gibt es auf einer Lichtung im Osten. Steine findest du am Berg im Südosten und Gras für Stroh solltest du am See im Süden bekommen.",
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
		}
		return new Dialog
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
	}
}
