using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
	[Header("Melee Weapon Settings")]
	[SerializeField] public float StunTime;
	[SerializeField] public float KnockbackAmount = 50;
	[SerializeField] protected Animator _meleeWeaponAnimator;

	public float SkillDamage;
	private List<int> _enemiesHit = new List<int>(); 

	protected virtual void UseWeapon()
	{
	}

	protected override void RotateWeapon()
	{
		// Do not rotate a melee weapon
	}

	public override bool IsMeleeWeapon()
	{
		return true;
	}

	public void ClearLastHitEnemies()
	{
		_enemiesHit.Clear();
	}

	public override bool IsMeleeWeaponAndBusy()
	{
		bool returnVal = !(_meleeWeaponAnimator.GetCurrentAnimatorStateInfo(0).IsName("New State"));

		if (returnVal)
			Debug.Log("The Melee Weapon Is Busy!");

		return returnVal;
	}
}
