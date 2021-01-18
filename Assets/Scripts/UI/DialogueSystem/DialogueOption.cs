using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueOption : MonoBehaviour
{
	[SerializeField] public TextMeshProUGUI Text;

	public enum ButtonActions { Nothing, StartArena, StartDungeon, OpenEquipsVendor, OpenSkillTeacher, LowerLevel, CurrentLevel, RaiseLevel }

	public ButtonActions buttonAction;
	public ButtonType buttonType;
	public int nextIndex = 0;

	private void Start()
	{
		if (buttonType == ButtonType.Action)
		{
			switch (buttonAction)
			{
				case ButtonActions.CurrentLevel:
					LevelManager.Instance.LevelBeforeArena = LevelManager.Instance.CurrentLevel;
					break;
			}
		}
	}

	private void Update()
	{
		if (buttonType == ButtonType.Action)
		{
			switch (buttonAction)
			{
				case ButtonActions.CurrentLevel:
					Text.text = LevelManager.Instance.CurrentLevel.ToString();
					break;
			}
		}

		float textPaddingSize = (15f);
		Vector2 backgorundSize = new Vector2(Text.preferredWidth + textPaddingSize * 2f, GetComponent<RectTransform>().sizeDelta.y);
		GetComponent<RectTransform>().sizeDelta = backgorundSize;
	}

	public virtual void Click()
	{
		if (buttonType == ButtonType.Continue)
			DialogueManager.Instance.DisplayNextSentence(nextIndex);
		else if (buttonType == ButtonType.End)
			DialogueManager.Instance.EndDialogue();
		else if (buttonType == ButtonType.Action)
		{
			HandleAction();
		}
	}

	private void HandleAction()
	{
		switch (buttonAction)
		{
			case ButtonActions.Nothing:
				break;

			case ButtonActions.StartArena:
				ArenaManager.Instance.StartArenaFromButton();
				DialogueManager.Instance.EndDialogue();
				break;

			case ButtonActions.StartDungeon:
				DungeonGenerator.Instance.StartDungeon();
				DialogueManager.Instance.EndDialogue();
				break;

			case ButtonActions.OpenEquipsVendor:
				ShopPanel.Instance.OpenShop();
				DialogueManager.Instance.EndDialogue();
				break;

			case ButtonActions.OpenSkillTeacher:
				SkillUnlockerManager.Instance.OpenShop();
				DialogueManager.Instance.EndDialogue();
				break;

			case ButtonActions.RaiseLevel:
				LevelManager.Instance.CurrentLevel++;
				break;

			case ButtonActions.LowerLevel:
				LevelManager.Instance.CurrentLevel--;
				break;

			case ButtonActions.CurrentLevel:
				break;
		}
	}
}
