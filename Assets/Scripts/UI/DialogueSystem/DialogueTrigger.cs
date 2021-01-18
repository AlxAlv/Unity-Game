using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	[SerializeField] public Dictionary<int, Dialogue> DialogueDictionary;
	public bool CanTrigger = false;

	[SerializeField] private GameObject _toolTip;

	private void Update()
	{
		if (_toolTip != null)
			_toolTip.SetActive(CanTrigger);
	}

	public void TriggerDialogue()
	{
		if (CanTrigger)
			DialogueManager.Instance.StartDialogue(DialogueDictionary);
		else
			Debug.Log("Too Far...");
	}
}
