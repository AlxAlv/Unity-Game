using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
	public TextMeshProUGUI NameText;
	public TextMeshProUGUI DialogueText;
	public Animator Animator;
	public GameObject OptionButton;
	public GameObject OptionsList;

	private Dictionary<int, Dialogue> _currentDialogueDictionary;
    private Coroutine _currentCoroutine = null;

    public void StartDialogue(Dictionary<int, Dialogue> dialogueDictionary)
    {
	    Animator.SetBool("IsOpen", true);

	    _currentDialogueDictionary = dialogueDictionary;

	    DisplayNextSentence(0);
    }

    public void DisplayNextSentence(int index)
    {
	    if (_currentCoroutine != null)
			StopCoroutine(_currentCoroutine);

        _currentCoroutine = StartCoroutine(TypeSentence(_currentDialogueDictionary[index]));
    }

    public void EndDialogue()
    {
	    Animator.SetBool("IsOpen", false);
    }

    IEnumerator TypeSentence(Dialogue dialogue)
    {
	    // Reset
	    DialogueText.text = "";
	    foreach (Transform child in OptionsList.transform)
	    {
		    GameObject.Destroy(child.gameObject);
	    }

		// Display
		NameText.text = dialogue.Name;

		foreach (DialogueButton button in dialogue.Buttons)
		{
			GameObject buttonPrefab = Instantiate(OptionButton, Vector3.zero, Quaternion.identity);

			buttonPrefab.transform.parent = OptionsList.transform;
			buttonPrefab.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

			buttonPrefab.GetComponentInChildren<TextMeshProUGUI>().text = button.ButtonMessage;

			DialogueOption diagButton = buttonPrefab.GetComponent<DialogueOption>();

			diagButton.buttonType = button.ButtonType;
			diagButton.nextIndex = button.IndexOfNextDialogue;
			diagButton.buttonAction = button.ButtonAction;
		}

		foreach (char letter in dialogue.Sentence.ToCharArray())
	    {
		    DialogueText.text += letter;
		    yield return null;
	    }
    }
}
