using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class DialogBox : VBoxContainer
{
	enum TextState
	{
		None,
		Displaying,
		Waiting,
	}

	const float TEXT_SPEED = 20.0f;

	RichTextLabel textBox;
	Button[] buttons;
	TextureRect arrow;
	AnimationPlayer arrowAnim;
	AudioStreamPlayer player;

	float visibleChars;
	TextState state = TextState.None;
	Dialog currentDialog;
	IList<DialogOption> currentOptions;

	public override void _Ready()
	{
		textBox = (RichTextLabel)GetNode("text_panel/text");
		buttons = new[] { "a", "b", "c" }.Select(b => (Button)GetNode($"hbox/vbox/button_{b}")).ToArray();
		arrow = (TextureRect)GetNode("text_panel/text/arrow");
		arrowAnim = (AnimationPlayer)GetNode("text_panel/text/anim");
		player = (AudioStreamPlayer)GetNode("audio");
	}

	public void DisplayDialog(Dialog dialog)
	{
		currentDialog = dialog;
		currentDialog.Script?.Invoke();
		currentOptions = dialog.Options.Where(o => o.Condition == null || o.Condition.Invoke()).ToList();
		textBox.Text = currentDialog.Text;
		textBox.VisibleCharacters = 0;
		for (int i = 0; i < currentOptions.Count && i < buttons.Length; i++)
		{
			buttons[i].Text = currentOptions[i].Text;
		}
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].Visible = false;
		}
		this.Visible = true;
		state = TextState.Displaying;
		visibleChars = 0;
		arrow.Visible = false;
		UI.Instance.SetActive(UIPart.Dialog, true);
	}

	public override void _Process(float delta)
	{
		if (state == TextState.Displaying)
		{
			visibleChars += TEXT_SPEED * delta;
			if (Input.IsActionJustPressed("ui_accept"))
			{
				visibleChars = textBox.Text.Length;
			}

			textBox.VisibleCharacters = Mathf.FloorToInt(visibleChars);
			if (visibleChars >= textBox.Text.Length)
			{
				state = TextState.Waiting;
				if (currentOptions.Count == 0)
				{
					arrow.Visible = true;
					arrowAnim.Play("Bounce");
				}
				else
				{
					for (int i = 0; i < currentOptions.Count && i < buttons.Length; i++)
					{
						buttons[i].Visible = true;
					}
					player.Stream = ResourceLoader.Load("res://sounds/dialog_open.ogg") as AudioStream;
					player.Play();
				}
			}
		}
		else if (state == TextState.Waiting)
		{
			if (Input.IsActionJustPressed("ui_accept") && currentOptions.Count == 0)
			{
				arrow.Visible = false;
				arrowAnim.Stop();
				DisplayOrExit(currentDialog.Next);
			}
		}
	}

	private void DisplayOrExit(Dialog dialog)
	{
		if (dialog != null)
		{
			DisplayDialog(dialog);
		}
		else
		{
			this.Visible = false;
			state = TextState.None;
			currentDialog = null;
			UI.Instance.SetActive(UIPart.Dialog, false);
		}
	}

	public void OnButtonPressed(int index)
	{
		if (state != TextState.Waiting) return;
		DisplayOrExit(currentOptions[index].Next);
		player.Stream = ResourceLoader.Load("res://sounds/dialog_choose.ogg") as AudioStream;
		player.Play();
	}
}
