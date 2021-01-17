using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType { Continue, Action, End }

public class DialogueButton
{
	public DialogueButton(string message, int indexOfNext, ButtonType buttonType, DialogueOption.ButtonActions action = DialogueOption.ButtonActions.Nothing)
	{
		ButtonMessage = message;
		IndexOfNextDialogue = indexOfNext;
		ButtonType = buttonType;
		ButtonAction = action;
	}

	public string ButtonMessage;
	public int IndexOfNextDialogue;
	public ButtonType ButtonType;
	public DialogueOption.ButtonActions ButtonAction;
}

public class Dialogue
{
	public enum Emotions { Happy, Neutral, Sad, Shocked, Angry, Dissapointed }

	public Emotions Emotion;
	public string Name;
	public string Sentence;
	public List<DialogueButton> Buttons;
}
