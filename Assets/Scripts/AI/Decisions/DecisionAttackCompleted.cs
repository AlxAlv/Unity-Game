using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/AttackCompleted", fileName = "DecisionAttackCompleted")]
public class DecisionAttackCompleted : AIDecision
{
	public override bool Decide(AIStateController controller)
	{
		return AttackCompleted(controller);
	}

	private bool AttackCompleted(AIStateController controller)
	{
		if (controller.EntityWeapon.CurrentWeapon.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length 
		> controller.EntityWeapon.CurrentWeapon.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime)
		{
			return true;
		}

		return false;
	}
}
