using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerSkill : BaseSkill
{
	[SerializeField] protected ParticleSystem _summonPS;

	protected NecromancerWeapon _necromancerWeaponToUse;

	public NecromancerSkill(NecromancerWeapon weaponToUse) : base(weaponToUse)
	{
		_necromancerWeaponToUse = weaponToUse;
	}

	public override bool IsBase()
	{
		return false;
	}
}
