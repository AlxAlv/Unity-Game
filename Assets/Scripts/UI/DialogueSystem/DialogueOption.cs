using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOption : MonoBehaviour
{
	public enum ButtonActions { Nothing, StartArena, StartDungeon, OpenEquipsVendor, OpenSkillTeacher }

	public ButtonActions buttonAction;
	public ButtonType buttonType;
	public int nextIndex = 0;

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
				DialogueManager.Instance.EndDialogue();
				break;

			case ButtonActions.OpenSkillTeacher:
				SkillUnlockerManager.Instance.OpenShop();
				DialogueManager.Instance.EndDialogue();
				break;
		}
	}
}
