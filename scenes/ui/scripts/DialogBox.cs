using Godot;
using System;
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

	float visibleChars;
	TextState state = TextState.None;
	Dialog currentDialog;

	public override void _Ready()
	{
		textBox = (RichTextLabel)GetNode("text_panel/text");
		buttons = new[] { "a", "b", "c" }.Select(b => (Button)GetNode($"hbox/vbox/button_{b}")).ToArray();
		arrow = (TextureRect)GetNode("text_panel/text/arrow");
		arrowAnim = (AnimationPlayer)GetNode("text_panel/text/anim");
	}

	public void DisplayDialog(Dialog dialog)
	{
		currentDialog = dialog;
		currentDialog.Script?.Invoke();
		textBox.Text = currentDialog.Text;
		for (int i = 0; i < currentDialog.Options.Count && i < buttons.Length; i++)
		{
			buttons[i].Text = currentDialog.Options.ElementAt(i).Key;
		}
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].Visible = false;
		}
		this.Visible = true;
		Input.SetMouseMode(Input.MouseMode.Visible);
		this.GetTree().Paused = true;
		state = TextState.Displaying;
		visibleChars = 0;
		arrow.Visible = false;
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
				if (currentDialog.Options.Count == 0)
				{
					arrow.Visible = true;
					arrowAnim.Play("Bounce");
				}
				else
				{
					for (int i = 0; i < currentDialog.Options.Count && i < buttons.Length; i++)
					{
						buttons[i].Visible = true;
					}
				}
			}
		}
		else if (state == TextState.Waiting)
		{
			if (Input.IsActionJustPressed("ui_accept") && currentDialog.Options.Count == 0)
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
			GetTree().Paused = false;
			state = TextState.None;
			Input.SetMouseMode(Input.MouseMode.Captured);
			currentDialog = null;
		}
	}

	public void OnButtonPressed(int index)
	{
		if (state != TextState.Waiting) return;
		DisplayOrExit(currentDialog.Options.ElementAt(index).Value);
	}
}
