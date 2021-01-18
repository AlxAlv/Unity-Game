using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaMaster : MonoBehaviour
{
	private DialogueTrigger _dialogueTrigger;
	private string name = "Arena Master";

    // Start is called before the first frame update
    void Start()
    {
	    _dialogueTrigger = GetComponent<DialogueTrigger>();

	    Dictionary<int, Dialogue> dialogueDictionary = new Dictionary<int, Dialogue>();

        // Setup
        // First Message
        Dialogue message0 = new Dialogue();
        List<DialogueButton> dialogueButtons0 = new List<DialogueButton>();

        message0.Emotion = Dialogue.Emotions.Angry;
        message0.Name = name;
        message0.Sentence = "Prove your worth to Valhalla by testing your might in the arena!";

        DialogueButton button0_0 = new DialogueButton("Sure!", 1, ButtonType.Continue);
        dialogueButtons0.Add(button0_0);

        DialogueButton button0_1 = new DialogueButton("Maybe Later...", 1, ButtonType.End);
        dialogueButtons0.Add(button0_1);

        message0.Buttons = dialogueButtons0;

        dialogueDictionary.Add(0, message0);

        // Second Message
        Dialogue message1 = new Dialogue();
        List<DialogueButton> dialogueButtons1 = new List<DialogueButton>();

        message1.Name = name;
        message1.Emotion = Dialogue.Emotions.Happy;
        message1.Sentence = "That's what I like to hear!\nWhat level monsters do you wish to face off with?";

        DialogueButton button1_1 = new DialogueButton("Lower!", 2, ButtonType.Action);
        button1_1.ButtonAction = DialogueOption.ButtonActions.LowerLevel;

        DialogueButton button1_2 = new DialogueButton(LevelManager.Instance.CurrentLevel.ToString(), 2, ButtonType.Action);
        button1_2.ButtonAction = DialogueOption.ButtonActions.CurrentLevel;

        DialogueButton button1_3 = new DialogueButton("Higher!", 2, ButtonType.Action);
        button1_3.ButtonAction = DialogueOption.ButtonActions.RaiseLevel;

        DialogueButton button1_4 = new DialogueButton("Perfect!", 2, ButtonType.Action);
        button1_4.ButtonAction = DialogueOption.ButtonActions.StartArena;

        dialogueButtons1.Add(button1_1);
        dialogueButtons1.Add(button1_2);
        dialogueButtons1.Add(button1_3);
        dialogueButtons1.Add(button1_4);

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
