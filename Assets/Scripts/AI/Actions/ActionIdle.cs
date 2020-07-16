using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Idle", fileName = "ActionIdle")]
public class ActionIdle : AIAction
{
	public override void Act(AIStateController controller)
	{
		// Do Nothing!
		controller.EntityMovement.StopAIMoving();

		// Remove Skill Loaded
		if (controller.EntityWeapon != null && controller.EntityWeapon.CurrentWeapon != null)
				controller.EntityWeapon.CurrentWeapon.CancelSkills();
	}
}
