using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBooksVendor : MonoBehaviour
{
    private DialogueTrigger _dialogueTrigger;
    private string name = "Skill Teacher";

    // Start is called before the first frame update
    void Start()
    {
        _dialogueTrigger = GetComponent<DialogueTrigger>();

        Dictionary<int, Dialogue> dialogueDictionary = new Dictionary<int, Dialogue>();

        // Setup
        // First Message
        Dialogue message0 = new Dialogue();
        List<DialogueButton> dialogueButtons0 = new List<DialogueButton>();

        message0.Emotion = Dialogue.Emotions.Happy;
        message0.Name = name;
        message0.Sentence = "I'm here to teach you new skills you can use in combat!\nWant to see what I have to offer?";

        DialogueButton button0_0 = new DialogueButton("Sure", 1, ButtonType.Action);
        button0_0.ButtonAction = DialogueOption.ButtonActions.OpenSkillTeacher;
        dialogueButtons0.Add(button0_0);

        DialogueButton button0_1 = new DialogueButton("I'm good, thanks.", 1, ButtonType.End);
        dialogueButtons0.Add(button0_1);

        message0.Buttons = dialogueButtons0;

        dialogueDictionary.Add(0, message0);

        // End
        _dialogueTrigger.DialogueDictionary = dialogueDictionary;
    }

    private void Update()
    {
        _dialogueTrigger.CanTrigger = true;
    }
}
