using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Follow", fileName = "ActionFollow")]
public class ActionFollow : AIAction
{
	public float MinDistanceToFollow = 1.0f;

	private Vector2 _aimDirection;

	public override void Act(AIStateController controller)
	{
		FollowTarget(controller);
		ThoughtPopups.Create(controller.SkillLoadingTransform.position, controller.gameObject.GetInstanceID(), ThoughtTypes.ExclamationMark);
	}

	private void FollowTarget(AIStateController controller)
	{
		SoundManager.Instance.UpdateCombatTimer();

		// Check if the target is not null
		if (controller.Target == null)
			return;

		controller.EntityMovement.SetAIFollow(controller.Target);

		if (controller.EntityWeapon.CurrentWeapon != null)
		{
			DetermineAim(controller);
			SetWeaponAim(controller, _aimDirection);
			controller.EntityWeapon.CurrentWeapon.CancelSkills();
		}
	}

	private void DetermineAim(AIStateController controller)
	{
		_aimDirection = controller.transform.position - controller.Target.position;
	}

	private void SetWeaponAim(AIStateController controller, Vector2 aim)
	{
		WeaponAim[] weaponAims = controller.GetWeaponAims();

		foreach (var weaponAim in weaponAims)
			weaponAim.SetAim(aim);
	}
}
