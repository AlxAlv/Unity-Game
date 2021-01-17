using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipsVendor : MonoBehaviour
{
    private DialogueTrigger _dialogueTrigger;
    private string name = "Equips Vendor";

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
        //message0.Sentence = "How's it going?\nI've got some nifty equips for sale.\nCare to look?";
        message0.Sentence = "Shop coming soon...";

        DialogueButton button0_0 = new DialogueButton("Lame.", 1, ButtonType.Action);
        button0_0.ButtonAction = DialogueOption.ButtonActions.OpenEquipsVendor;
        dialogueButtons0.Add(button0_0);

        //DialogueButton button0_1 = new DialogueButton("No thanks.", 1, ButtonType.End);
        //dialogueButtons0.Add(button0_1);

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
