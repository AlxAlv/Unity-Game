using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMaster : MonoBehaviour
{
	private DialogueTrigger _dialogueTrigger;
	private string name = "Dungeon Master";

	// Start is called before the first frame update
	void Start()
	{
		_dialogueTrigger = GetComponent<DialogueTrigger>();

		Dictionary<int, Dialogue> dialogueDictionary = new Dictionary<int, Dialogue>();

		// Setup
		// First Message
		Dialogue message0 = new Dialogue();
		List<DialogueButton> dialogueButtons0 = new List<DialogueButton>();

		message0.Emotion = Dialogue.Emotions.Shocked;
		message0.Name = name;
		message0.Sentence = "Monsters are trying to claw their way up to Valhalla!\nWill you assist in destroying them?";

		DialogueButton button0_0 = new DialogueButton("Sure!", 1, ButtonType.Continue);
		dialogueButtons0.Add(button0_0);

		DialogueButton button0_1 = new DialogueButton("Not now...", 1, ButtonType.End);
		dialogueButtons0.Add(button0_1);

		message0.Buttons = dialogueButtons0;

		dialogueDictionary.Add(0, message0);

		// Second Message
		Dialogue message1 = new Dialogue();
		List<DialogueButton> dialogueButtons1 = new List<DialogueButton>();

		message1.Name = name;
		message1.Emotion = Dialogue.Emotions.Happy;
		message1.Sentence = "Thank you so much!\nJust so you know, descending a floor will cause all future enemies to be stronger than before!";

		DialogueButton button1_2 = new DialogueButton("I'm on it!", 2, ButtonType.Action);
		button1_2.ButtonAction = DialogueOption.ButtonActions.StartDungeon;

		DialogueButton button1_1 = new DialogueButton("Actually, nevermind...", 1, ButtonType.End);
		dialogueButtons1.Add(button1_2);
		dialogueButtons1.Add(button1_1);

		message1.Buttons = dialogueButtons1;

		dialogueDictionary.Add(1, message1);

		// End
		_dialogueTrigger.DialogueDictionary = dialogueDictionary;
	}

	private void Update()
	{
		_dialogueTrigger.CanTrigger = true;
	}
}

