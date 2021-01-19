using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenrirManager : MonoBehaviour
{
	private DialogueTrigger _dialogueTrigger;
	private string name = "Fenrir";

    // Start is called before the first frame update
    void Start()
    {
        _dialogueTrigger = GetComponent<DialogueTrigger>();

        Dictionary<int, Dialogue> dialogueDictionary = new Dictionary<int, Dialogue>();

        // Setup (0)
        // First Message
        Dialogue message0 = new Dialogue();
        List<DialogueButton> dialogueButtons0 = new List<DialogueButton>();

        message0.Emotion = Dialogue.Emotions.Happy;
        message0.Name = name;
        message0.Sentence = "Hello, I'm Fenrir!\nI'm here to to help answer any question I have knowledge on!\nJust Press [H] To talk to me.";

        DialogueButton button0_0 = new DialogueButton("I Need Help!", 1, ButtonType.Continue);
        dialogueButtons0.Add(button0_0);

        DialogueButton button0_1 = new DialogueButton("Thanks, I will.", 1, ButtonType.End);
        dialogueButtons0.Add(button0_1);

        message0.Buttons = dialogueButtons0;

        dialogueDictionary.Add(0, message0);

        // What is There To Do Message (1)
        // First Message
        Dialogue message1 = new Dialogue();
        List<DialogueButton> dialogueButtons1 = new List<DialogueButton>();

        message1.Emotion = Dialogue.Emotions.Shocked;
        message1.Name = name;
        message1.Sentence = "What would you like to know more about?";

        DialogueButton button1_0 = new DialogueButton("The Arena", 2, ButtonType.Continue);
        dialogueButtons1.Add(button1_0);

        DialogueButton button1_1 = new DialogueButton("The Dungeon", 3, ButtonType.Continue);
        dialogueButtons1.Add(button1_1);

        DialogueButton button1_2 = new DialogueButton("Controls", 4, ButtonType.Continue);
        dialogueButtons1.Add(button1_2);

        message1.Buttons = dialogueButtons1;

        dialogueDictionary.Add(1, message1);

        // Arena Info (2)
        // First Message
        Dialogue message2 = new Dialogue();
        List<DialogueButton> dialogueButtons2 = new List<DialogueButton>();

        message2.Emotion = Dialogue.Emotions.Happy;
        message2.Name = name;
        message2.Sentence = "An arena match is composed of 5 random objectives. After setting a difficulty, complete all the objectives to earn yourself some money!";

        DialogueButton button2_0 = new DialogueButton("Got it!", 2, ButtonType.End);
        dialogueButtons2.Add(button2_0);

        message2.Buttons = dialogueButtons2;

        dialogueDictionary.Add(2, message2);

        // Dungeon Info (3)
        // First Message
        Dialogue message3 = new Dialogue();
        List<DialogueButton> dialogueButtons3 = new List<DialogueButton>();

        message3.Emotion = Dialogue.Emotions.Happy;
        message3.Name = name;
        message3.Sentence = "The dungeon is an area where you fight enemies in search of the exit. At each exit you may choose to fight a boss and leave or descend another level.";

        DialogueButton button3_0 = new DialogueButton("[CONTINUE]", 5, ButtonType.Continue);
        dialogueButtons3.Add(button3_0);

        message3.Buttons = dialogueButtons3;

        dialogueDictionary.Add(3, message3);

        // Combat Info (4)
        // Combat Info
        Dialogue message4 = new Dialogue();
        List<DialogueButton> dialogueButtons4 = new List<DialogueButton>();

        message4.Emotion = Dialogue.Emotions.Happy;
        message4.Name = name;
        message4.Sentence = "[A] And [D] switches skillsets.\n[S] Targets the nearest enemy.\nLeft and right clicking, while targeting, loads a skill which can then be used once loaded.\n";

        DialogueButton button4_0 = new DialogueButton("[CONTINUE]", 6, ButtonType.Continue);
        dialogueButtons4.Add(button4_0);

        message4.Buttons = dialogueButtons4;

        dialogueDictionary.Add(4, message4);

        // Dungeon Info (5)
        // Dungeon Info 2
        Dialogue message5 = new Dialogue();
        List<DialogueButton> dialogueButtons5 = new List<DialogueButton>();

        message5.Emotion = Dialogue.Emotions.Sad;
        message5.Name = name;
        message5.Sentence = "Note that a boss fight or descending a floor permanently increases enemy levels in the dungeon. If you die, the dungeon level and yours are reset!";

        DialogueButton button5_0 = new DialogueButton("Got it!", 4, ButtonType.End);
        dialogueButtons5.Add(button5_0);

        message5.Buttons = dialogueButtons5;

        dialogueDictionary.Add(5, message5);

        // Combat Info (6)
        Dialogue message6 = new Dialogue();
        List<DialogueButton> dialogueButtons6 = new List<DialogueButton>();

        message6.Emotion = Dialogue.Emotions.Happy;
        message6.Name = name;
        message6.Sentence = "Holding [LEFT-SHIFT] reduces incoming damage.\nLetting go of [LEFT-SHIFT] has you dodge and avoid all damage.\n[ESC] Cancels any loaded skill.";

        DialogueButton button6_0 = new DialogueButton("[CONTINUE]", 7, ButtonType.Continue);
        dialogueButtons6.Add(button6_0);

        message6.Buttons = dialogueButtons6;

        dialogueDictionary.Add(6, message6);

        // Combat Info (7)
        Dialogue message7 = new Dialogue();
        List<DialogueButton> dialogueButtons7 = new List<DialogueButton>();

        message7.Emotion = Dialogue.Emotions.Happy;
        message7.Name = name;
        message7.Sentence = "And Finally...\nHolding [X] makes you run.\nPressing [V] opens chests.";

        DialogueButton button7_0 = new DialogueButton("Got it!", 4, ButtonType.End);
        dialogueButtons7.Add(button7_0);

        message7.Buttons = dialogueButtons7;

        dialogueDictionary.Add(7, message7);

        // End
        _dialogueTrigger.DialogueDictionary = dialogueDictionary;

        _dialogueTrigger.CanTrigger = true;
        _dialogueTrigger.TriggerDialogue();
    }

    void Update()
    {
	    if (Input.GetKeyDown(KeyCode.H))
	    {
		    _dialogueTrigger.TriggerDialogue();
        }
    }
}
