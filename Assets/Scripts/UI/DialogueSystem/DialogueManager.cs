using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
	public Image EmotionImage;
	public TextMeshProUGUI NameText;
	public TextMeshProUGUI DialogueText;
	public Animator Animator;
	public GameObject OptionButton;
	public GameObject OptionsList;

	private Dictionary<int, Dialogue> _currentDialogueDictionary;
    private Coroutine _currentCoroutine = null;

    // Constant Strings To Display To The Player
    private Dictionary<Dialogue.Emotions, string> _emojiDictionary = new Dictionary<Dialogue.Emotions, string>()
    {
	    {Dialogue.Emotions.Happy, "Sprites/Dialogue/emoji_happy"},
	    {Dialogue.Emotions.Sad, "Sprites/Dialogue/emoji_sad"},
	    {Dialogue.Emotions.Angry, "Sprites/Dialogue/emoji_mad"},
	    {Dialogue.Emotions.Neutral, "Sprites/Dialogue/emoji_neutral"},
	    {Dialogue.Emotions.Shocked, "Sprites/Dialogue/emoji_shocked"},
	    {Dialogue.Emotions.Dissapointed, "Sprites/Dialogue/emoji_dissapointed"}
	};

	public void StartDialogue(Dictionary<int, Dialogue> dialogueDictionary)
	{
		if (Animator.GetBool("IsOpen"))
			return;

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

  		Sprite tempSprite = Resources.Load<Sprite>(_emojiDictionary[dialogue.Emotion]);
		if (EmotionImage.sprite != tempSprite)
		{
			EmotionImage.sprite = Resources.Load<Sprite>(_emojiDictionary[dialogue.Emotion]);
			UIBounce.Instance.BounceUI(EmotionImage.gameObject);
		}

		foreach (char letter in dialogue.Sentence.ToCharArray())
	    {
		    DialogueText.text += letter;
		    yield return null;
	    }
    }
}
